using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace Retail_Managment_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for AdminDashbordView.xaml
    /// </summary>
    public partial class AdminDashbordView : Window
    {
        public AdminDashbordView()
        {
            InitializeComponent();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dashboard Clicked", "Dashboard", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Users Clicked", "Users", MessageBoxButton.OK, MessageBoxImage.Information);
            ManageUserView manageUserView = new ManageUserView();
            manageUserView.Show();
            this.Close();    
        }

        private void Items_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Items Clicked", "Items", MessageBoxButton.OK, MessageBoxImage.Information);
            ManageItemView manageItemView = new ManageItemView();
            manageItemView.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging Out...", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
            
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }

        // Enable window dragging
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeButton.Content = "□"; // Restore symbol
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizeButton.Content = "❐"; // Indicate maximized state
            }
        }
    }
}
