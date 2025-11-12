using System.Windows;
using FashionStoreManagement.Models; 
using System.Linq; 

namespace FashionStoreManagement
{
    public partial class LoginWindow : Window
    {
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null && (user.Role == "Admin" || user.Role == "Staff"))
            {
                ProductsWindow mainWindow = new ProductsWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username, password, or you do not have permission (Admin/Staff).", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}