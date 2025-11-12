using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FashionStoreManagement.Models;
using System.Linq;
using System.Collections.Generic; 
using System; 

namespace FashionStoreManagement
{
    public partial class ProductsWindow : Window, INotifyPropertyChanged
    {
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();

        
        private List<Product> _allProducts = new List<Product>(); 
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

        // Pagination Properties 
        private int _currentPage = 1;
        private int _itemsPerPage = 5; 
        private int _totalPages;
        private readonly User _loggedInUser;
        public ProductsWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            txtRoleDisplay.Text = _loggedInUser.Role;
            this.DataContext = this;
            LoadAllData();
            UpdateDataGrid();
        }

        //Load Data from Database
        private void LoadAllData()
        {
            _allProducts = _context.Products.Where(p => p.Status != "Archived").ToList();

            var categories = _allProducts.Select(p => p.Category).Distinct().ToList();
            Categories.Clear();
            Categories.Add("All Categories");
            categories.ForEach(c => Categories.Add(c));
            Products.Clear();
        }

        //Search bar 
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1; // Reset về trang 1
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            IEnumerable<Product> filteredProducts = _allProducts;
            // Apply Category filter
            if (cmbCategoryFilter != null && cmbCategoryFilter.SelectedItem != null)
            {
                string? selectedCategory = cmbCategoryFilter?.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "All Categories")
                {
                    filteredProducts = filteredProducts.Where(p => p.Category == selectedCategory);
                }
                if (selectedCategory != "All Categories")
                {
                    filteredProducts = filteredProducts.Where(p => p.Category == selectedCategory);
                }
            }
            //Search 
            string searchTerm = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.ProductName.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm))
                );
            }

            // Pagination logic
            _totalPages = (int)Math.Ceiling(filteredProducts.Count() / (double)_itemsPerPage);
            var paginatedProducts = filteredProducts
                                        .Skip((_currentPage - 1) * _itemsPerPage)
                                        .Take(_itemsPerPage)
                                        .ToList();

            // update ObservableCollection for DataGrid
            Products.Clear();
            paginatedProducts.ForEach(p => Products.Add(p));
        }


        // Event Handlers

        // Filter by Category
        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPage = 1;
            UpdateDataGrid();
        }

        // Paging 
        private void PagingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content?.ToString(), out int pageNumber))
            {
                _currentPage = pageNumber;
                UpdateDataGrid();
            }
        }

        // Previous page button
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdateDataGrid();
            }
        }

        // Next page button
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                UpdateDataGrid();
            }
        }

        // Edit Product
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Product productToEdit)
            {
                ProductDetailsWindow editWindow = new ProductDetailsWindow(productToEdit);
                if (editWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    _allProducts = _context.Products.Where(p => p.Status != "Archived").ToList();
                    UpdateDataGrid();
                }
            }
        }


        //Delete (Deactivate) Product

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Product productToDeactivate)
            {
                var result = MessageBox.Show($"Are you sure you want to archive {productToDeactivate.ProductName}?",
                                             "Confirm Archive",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    productToDeactivate.Status = "Archived";
                    _context.SaveChanges();
                    _allProducts.Remove(productToDeactivate);
                    UpdateDataGrid();
                }
            }
        }
        //  Add New Product
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailsWindow addWindow = new ProductDetailsWindow();
            if (addWindow.ShowDialog() == true)
            {
                Product newProduct = addWindow.NewProduct;
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                _allProducts.Add(newProduct);
                UpdateDataGrid();
            }
        }

      

        private bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
            else
            {
                this.DragMove();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}