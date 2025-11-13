using FashionStoreManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore; 

namespace FashionStoreManagement.Pages
{
    public partial class DashboardView : UserControl, INotifyPropertyChanged
    {
        private readonly FashionStoreDbContext _context = new FashionStoreDbContext();
        private int _lowStockThreshold = 10; 

       
        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get { return _totalRevenue; }
            set { _totalRevenue = value; OnPropertyChanged(nameof(TotalRevenue)); }
        }

        private int _ordersToday;
        public int OrdersToday
        {
            get { return _ordersToday; }
            set { _ordersToday = value; OnPropertyChanged(nameof(OrdersToday)); }
        }

        private int _totalCustomers;
        public int TotalCustomers
        {
            get { return _totalCustomers; }
            set { _totalCustomers = value; OnPropertyChanged(nameof(TotalCustomers)); }
        }

        private int _lowStockItemsCount;
        public int LowStockItemsCount
        {
            get { return _lowStockItemsCount; }
            set { _lowStockItemsCount = value; OnPropertyChanged(nameof(LowStockItemsCount)); }
        }

       
        public ObservableCollection<Order> RecentOrders { get; set; }
        public ObservableCollection<Product> LowStockProducts { get; set; }


        public DashboardView()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            TotalRevenue = _context.Orders.Sum(o => o.TotalAmount) ?? 0;
            OrdersToday = _context.Orders.Count(o => o.OrderDate.HasValue && o.OrderDate.Value.Date == DateTime.Today);
            TotalCustomers = _context.Customers.Count();
            LowStockItemsCount = _context.Products.Count(p => p.Status == "Active" && p.Quantity < _lowStockThreshold);

            // load 10 recent orders
            var recentOrdersData = _context.Orders
                                        .Include(o => o.Customer) 
                                        .OrderByDescending(o => o.OrderDate)
                                        .Take(10)
                                        .ToList();
            RecentOrders = new ObservableCollection<Order>(recentOrdersData);

            // load 10 low stock products
            var lowStockData = _context.Products
                                .Where(p => p.Status == "Active" && p.Quantity < _lowStockThreshold)
                                .OrderBy(p => p.Quantity)
                                .Take(10)
                                .ToList();
            LowStockProducts = new ObservableCollection<Product>(lowStockData);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}