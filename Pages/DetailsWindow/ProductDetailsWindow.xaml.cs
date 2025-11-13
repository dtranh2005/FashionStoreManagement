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
using FashionStoreManagement.Models;

namespace FashionStoreManagement.Pages.DetailsWindow
{
   
    
public partial class ProductDetailsWindow : Window
    {
        public Product NewProduct { get; private set; }

        private readonly Product? _productToEdit; 

        public ProductDetailsWindow()
        {
            InitializeComponent();
            NewProduct = new Product();
        }

        public ProductDetailsWindow(Product productToEdit)
        {
            InitializeComponent();
            _productToEdit = productToEdit;
            lblTitle.Text = "Edit Product";
            LoadProductData();
        }
        private void LoadProductData()
        {
            if (_productToEdit == null) return;
            txtProductId.Text = _productToEdit.ProductId.ToString();
            txtProductName.Text = _productToEdit.ProductName;
            txtCategory.Text = _productToEdit.Category;
            txtPrice.Text = _productToEdit.Price.ToString();
            txtQuantity.Text = _productToEdit.Quantity.ToString();
            txtDescription.Text = _productToEdit.Description;
        }

        // Save button
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation 
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                !decimal.TryParse(txtPrice.Text, out decimal price) ||
                !int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Invalid data. Please check Name, Price, and Quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check add or edit mode
            if (_productToEdit == null)
            {
                // Add mode: 
                NewProduct.ProductName = txtProductName.Text.Trim();
                NewProduct.Category = txtCategory.Text.Trim();
                NewProduct.Price = price;
                NewProduct.Quantity = quantity;
                NewProduct.Description = txtDescription.Text.Trim();
                NewProduct.Status = "Active"; 
            }
            else
            {
                // Edit mode:
                _productToEdit.ProductName = txtProductName.Text.Trim();
                _productToEdit.Category = txtCategory.Text.Trim();
                _productToEdit.Price = price;
                _productToEdit.Quantity = quantity;
                _productToEdit.Description = txtDescription.Text.Trim();
            }

            // Close dialog with OK result
            this.DialogResult = true;
        }


    }
    }
