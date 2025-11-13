using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FashionStoreManagement.Models;
using FashionStoreManagement.Pages.DetailsWindow;

// SỬA: Đổi Namespace
namespace FashionStoreManagement.Pages
{
  
    public partial class CustomersView : UserControl, INotifyPropertyChanged
    {
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();

        private List<Customer> _allCustomers = new List<Customer>();

        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        private int _currentPage = 1;
        private int _itemsPerPage = 5;
        private int _totalPages;


        public CustomersView()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadAllData();
            UpdateDataGrid();
        }

        // SỬA: Logic Load
        private void LoadAllData()
        {
            // (Hiện tại Customer model không có Status, nên chúng ta tải tất cả)
            _allCustomers = _context.Customers.ToList();
            Customers.Clear();
        }

        private void UpdateDataGrid()
        {
            IEnumerable<Customer> filteredCustomers = _allCustomers;

       
            string searchTerm = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredCustomers = filteredCustomers.Where(c =>
                    c.FullName.ToLower().Contains(searchTerm) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchTerm))
                );
            }

           
            string addressFilterTerm = txtAddressFilter.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(addressFilterTerm))
            {
                filteredCustomers = filteredCustomers.Where(c =>
                    c.Address != null && c.Address.ToLower().Contains(addressFilterTerm)
                );
            }

            _totalPages = (int)Math.Ceiling(filteredCustomers.Count() / (double)_itemsPerPage);
            var paginatedCustomers = filteredCustomers
                                        .Skip((_currentPage - 1) * _itemsPerPage)
                                        .Take(_itemsPerPage)
                                        .ToList();

            Customers.Clear();
            paginatedCustomers.ForEach(c => Customers.Add(c));
        }


     
        private void txtAddressFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            UpdateDataGrid();
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            UpdateDataGrid();
        }

      
        private void PagingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content?.ToString(), out int pageNumber))
            {
                _currentPage = pageNumber;
                UpdateDataGrid();
            }
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1) { _currentPage--; UpdateDataGrid(); }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages) { _currentPage++; UpdateDataGrid(); }
        }


        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Customer customerToEdit)
            {
               
                 CustomerDetailsWindow editWindow = new CustomerDetailsWindow(customerToEdit);
                if (editWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    _allCustomers = _context.Customers.ToList();
                   UpdateDataGrid();
                 }
                MessageBox.Show($"Đang sửa khách hàng: {customerToEdit.FullName}");
            }
        }

      
        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Customer customerToDelete)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {customerToDelete.FullName}?",
                                             "Confirm Delete",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {       
                    try
                    {
                        _context.Customers.Remove(customerToDelete);
                        _context.SaveChanges();
                        _allCustomers.Remove(customerToDelete);
                        UpdateDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting customer: {ex.InnerException?.Message ?? ex.Message}", "Database Error");
                    }
                }
            }
        }

       
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            
             CustomerDetailsWindow addWindow = new CustomerDetailsWindow();
             if (addWindow.ShowDialog() == true)
             {
                Customer newCustomer = addWindow.NewCustomer;
                _context.Customers.Add(newCustomer);
                _context.SaveChanges();
                _allCustomers.Add(newCustomer);
               UpdateDataGrid();
            }
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}