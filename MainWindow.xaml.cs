using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives; 
using System.Windows.Input;
using FashionStoreManagement.Models; 
 using FashionStoreManagement.Pages; 

namespace FashionStoreManagement
{
    public partial class MainWindow : Window
    {
        private readonly User _loggedInUser;

        public MainWindow(User loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            fContainer.Navigate(new DashboardView());
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "btnDashboard":
                        fContainer.Navigate(new DashboardView());
                        break;
                    case "btnProducts":
                        fContainer.Navigate(new ProductsView());
                        break;
                    case "btnCustomers":
                        fContainer.Navigate(new CustomersView());
                        break;
                    case "btnOrders":
                        MessageBox.Show("Chưa tạo trang Orders!");
                        break;
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void btnMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false) 
            {
                if (sender is Button button && button.Tag is string tooltipText)
                {
                    Popup.PlacementTarget = button;
                    Popup.Placement = PlacementMode.Right;
                    Popup.IsOpen = true;
                  
                }
            }
        }

        private void btnMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
    }
}