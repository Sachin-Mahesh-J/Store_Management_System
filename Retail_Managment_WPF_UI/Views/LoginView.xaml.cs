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

namespace Retail_Managment_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (username == "admin" && password == "1234") // Replace with DB authentication
            {
                AdminDashbordView adminDashbordView = new AdminDashbordView();
                adminDashbordView.Show();
                
            }
            else if(username == "user" && password == "1234") 
            {
                UserDashbordView userDashView = new UserDashbordView();
                userDashView.Show();
                this.Close();
            }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
