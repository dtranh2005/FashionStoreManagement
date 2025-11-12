using System.Windows;
using FashionStoreManagement.Models;

namespace FashionStoreManagement
{
    public partial class ProductDetailsWindow : Window
    {
        public Product NewProduct { get; private set; } = null!;
        private readonly Product? _productToEdit;

        public ProductDetailsWindow()
        {
            InitializeComponent();
        }

        public ProductDetailsWindow(Product productToEdit)
        {
            InitializeComponent();
            _productToEdit = productToEdit;
            Title = "Edit Product";
            LoadProductData();
        }

        private void LoadProductData()
        {
            if (_productToEdit == null) return;

            txtProductName.Text = _productToEdit.ProductName;
            txtCategory.Text = _productToEdit.Category;
            txtPrice.Text = _productToEdit.Price.ToString();
            txtQuantity.Text = _productToEdit.Quantity.ToString();
            txtDescription.Text = _productToEdit.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                !decimal.TryParse(txtPrice.Text, out decimal price) ||
                !int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Please enter valid data. Product Name, Price, and Quantity are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_productToEdit == null)
            {
                NewProduct = new Product
                {
                    ProductName = txtProductName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Price = price,
                    Quantity = quantity,
                    Description = txtDescription.Text.Trim()
                };
            }
            else
            {
                _productToEdit.ProductName = txtProductName.Text.Trim();
                _productToEdit.Category = txtCategory.Text.Trim();
                _productToEdit.Price = price;
                _productToEdit.Quantity = quantity;
                _productToEdit.Description = txtDescription.Text.Trim();
            }

            this.DialogResult = true;
        }
    }
}   