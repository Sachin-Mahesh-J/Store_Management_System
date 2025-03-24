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
using Retail_Managment_WPF_UI.Services;

namespace Retail_Managment_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for ManaageProductsView.xaml
    /// </summary>
    public partial class ManaageProductsView : Window
    {
        public ManaageProductsView()
        {
            InitializeComponent();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            CommonButtons.minimizebutton(this);
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            CommonButtons.maximizebutton(this, MaximizeButton);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CommonButtons.closebutton(this);
        }

        private void topbar_darg(object sender, MouseButtonEventArgs e)
        {
            CommonButtons.dragmove(this, e);
        }

        private void btnM_employee_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Admin_ManageEmployeesView();
            window.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {

        }

        private void btnM_Porduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
