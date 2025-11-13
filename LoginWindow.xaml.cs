using FashionStoreManagement.Models;
using FashionStoreManagement.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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


namespace FashionStoreManagement
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            txtErrorMessage.Text = "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtErrorMessage.Text = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                return;
            }

            try
            {

                using (var db = new FashionStoreDbContext())
                {
                    var user = await db.Users
                                     .FirstOrDefaultAsync(u => u.Username == username);

                    if (user != null)
                    {
                        if (user.Password == password)
                        {
                            MainWindow mainWindow = new MainWindow(user);
                            mainWindow.Show(); 
                            this.Close();
                        }
                        else
                        {
                            txtErrorMessage.Text = "Incorrect username or pasword";
                        }
                    }
                    else
                    {
                        txtErrorMessage.Text = "Incorrect username or pasword";
                    }
                }
            }
            catch (Exception ex)
            {
                txtErrorMessage.Text = "Cannot connect to database";
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
