using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FashionStoreManagement.Models;
using FashionStoreManagement.Pages;
using FashionStoreManagement.Pages.DetailsWindow;

namespace FashionStoreManagement.Pages
{
    public partial class ProductsView : UserControl, INotifyPropertyChanged
    {
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();

        private List<Product> _allProducts = new List<Product>();
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();



        // Pagination Properties 
        private int _currentPage = 1;
        private int _itemsPerPage = 5;
        private int _totalPages;

        public ProductsView()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadAllData();
            UpdateDataGrid();
        }

        //Load Data from Database
        private void LoadAllData()
        {

            _allProducts = _context.Products.Where(p => p.Status != "Archived").ToList();
            Products.Clear();
        }

        private void UpdateDataGrid()
        {
            IEnumerable<Product> filteredProducts = _allProducts;

            // Apply Search filter (Thanh tìm kiếm chính)
            string searchTerm = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.ProductName.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm))
                );
            }

            // Apply Category filter (Thanh tìm kiếm Category)
            string categoryFilterTerm = txtCategoryFilter.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(categoryFilterTerm))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Category != null && p.Category.ToLower().Contains(categoryFilterTerm)
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
        private void txtCategoryFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            UpdateDataGrid();
        }

        // Search Bar
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
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

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}