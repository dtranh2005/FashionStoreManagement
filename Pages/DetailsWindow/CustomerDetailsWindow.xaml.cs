using FashionStoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FashionStoreManagement.Pages.DetailsWindow
{
    public partial class CustomerDetailsWindow : Window
    {
        public Customer NewCustomer { get; private set; }
        private readonly Customer? _customerToEdit;
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();

        // Constructor cho chế độ "Add"
        public CustomerDetailsWindow()
        {
            InitializeComponent();
            NewCustomer = new Customer();
        }

        // Constructor cho chế độ "Edit"
        public CustomerDetailsWindow(Customer customerToEdit)
        {
            InitializeComponent();
            _customerToEdit = customerToEdit;
            lblTitle.Text = "Edit Customer";
            LoadCustomerData();
        }

        // Tải dữ liệu (cho chế độ "Edit")
        private void LoadCustomerData()
        {
            if (_customerToEdit == null) return;

            txtCustomerId.Text = _customerToEdit.CustomerId.ToString();
            txtCustomerId.IsEnabled = false; // Không cho sửa Khóa Chính

            txtFullName.Text = _customerToEdit.FullName;
            txtPhone.Text = _customerToEdit.Phone;
            txtEmail.Text = _customerToEdit.Email;
            txtAddress.Text = _customerToEdit.Address;
        }

        // Nút Save
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation
            if (!int.TryParse(txtCustomerId.Text, out int customerId) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Invalid data. Please check ID and Full Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Kiểm tra chế độ (Add/Edit)
            if (_customerToEdit == null)
            {
                // CHẾ ĐỘ ADD
                // Kiểm tra ID trùng lặp
                if (_context.Customers.Any(c => c.CustomerId == customerId))
                {
                    MessageBox.Show($"Customer ID {customerId} already exists.", "Duplicate ID", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewCustomer.CustomerId = customerId;
                NewCustomer.FullName = txtFullName.Text.Trim();
                NewCustomer.Phone = txtPhone.Text.Trim();
                NewCustomer.Email = txtEmail.Text.Trim();
                NewCustomer.Address = txtAddress.Text.Trim();
            }
            else
            {
                // CHẾ ĐỘ EDIT
                _customerToEdit.FullName = txtFullName.Text.Trim();
                _customerToEdit.Phone = txtPhone.Text.Trim();
                _customerToEdit.Email = txtEmail.Text.Trim();
                _customerToEdit.Address = txtAddress.Text.Trim();
            }

            // 3. Đóng
            this.DialogResult = true;
        }
    }
}
