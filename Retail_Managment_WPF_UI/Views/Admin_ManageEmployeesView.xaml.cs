using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MySql.Data.MySqlClient;
using Retail_Managment_WPF_UI.Services;

namespace Retail_Managment_WPF_UI.Views
{
    /// <summary>
    /// Interaction logic for ManageEmployeesView.xaml
    /// </summary>
    public partial class Admin_ManageEmployeesView : Window
    {
        private Employee _currentEmployee;
        private bool _isEditMode = false;
        private ObservableCollection<Employee> _allEmployees = new ObservableCollection<Employee>();
        private ObservableCollection<Employee> _filteredEmployees = new ObservableCollection<Employee>();

        public Admin_ManageEmployeesView()
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

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password=txtPassword.Password;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string role = cmbRole.Text;
            Console.WriteLine(role);

            Employee employee = new Employee(username, password, phone, email, role);
            if (employee.verifyEnteredData(this, cmbRole))
            {
                if (employee.AddEmployee(this))
                {
                    employee.LoadEmployees(employeeDataGrid);
                    txtUsername.Text = "";
                    txtPassword.Password = "";
                    txtEmail.Text = "";
                    txtPhone.Text = "";
                    cmbRole.SelectedIndex = -1;
                    return;
                }
            }
        }

        private void btnM_employee_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Admin_ManageEmployeesView();
            window.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee();
            employee.LoadEmployees(employeeDataGrid);
        }


        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            Employee selectedEmployee = editButton.DataContext as Employee;

            if (selectedEmployee != null)
            {
                _currentEmployee = Employee.GetEmployeeById(selectedEmployee.UserID);
                if (_currentEmployee != null)
                {
                    // Populate form fields
                    txtUsername.Text = _currentEmployee.Username;
                    txtEmail.Text = _currentEmployee.Email;
                    txtPhone.Text = _currentEmployee.Phone;

                    // Set ComboBox selection
                    foreach (ComboBoxItem item in cmbRole.Items)
                    {
                        if (item.Content.ToString() == _currentEmployee.Role)
                        {
                            cmbRole.SelectedItem = item;
                            break;
                        }
                    }

                    _isEditMode = true;
                }
            }
        }
        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null || !_isEditMode)
            {
                MessageBox.Show("No employee selected for editing.");
                return;
            }

            // Get updated values
            string newPassword = txtPassword.Password;
            string role = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Update employee properties
            _currentEmployee.Username = txtUsername.Text;
            _currentEmployee.Email = txtEmail.Text;
            _currentEmployee.Phone = txtPhone.Text;
            _currentEmployee.Role = role;

            // Update password only if provided
            if (!string.IsNullOrEmpty(newPassword))
            {
                _currentEmployee.HashedPassword = _currentEmployee.HashPassword(newPassword);
            }

            if (_currentEmployee.UpdateEmployee(this))
            {
                // Refresh data grid
                Employee employee = new Employee();
                employee.LoadEmployees(employeeDataGrid);

                // Clear form
                txtUsername.Text = "";
                txtPassword.Password = "";
                txtEmail.Text = "";
                txtPhone.Text = "";
                cmbRole.SelectedIndex = -1;

                _isEditMode = false;
                _currentEmployee = null;
            }
        }

        private void topbar_darg(object sender, MouseButtonEventArgs e)
        {
            CommonButtons.dragmove(this, e);
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Employee selectedEmployee = deleteButton.DataContext as Employee;

            if (selectedEmployee != null)
            {
                DeleteEmployee(selectedEmployee.UserID);
            }
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployees = employeeDataGrid.SelectedItems.Cast<Employee>().ToList();

            if (selectedEmployees.Count == 0)
            {
                MessageBox.Show("Please select at least one employee to delete.",
                               "Warning",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {selectedEmployees.Count} selected employee(s)?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                List<int> userIds = selectedEmployees.Select(emp => emp.UserID).ToList();
                Employee employee = new Employee();

                if (employee.DeleteEmployees(this, userIds))
                {
                    // Refresh the DataGrid
                    employee.LoadEmployees(employeeDataGrid);

                    // Clear selection
                    employeeDataGrid.UnselectAll();
                }
            }
        }

        private void DeleteEmployee(int userId)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this employee?",
                                                    "Confirm Delete",
                                                    MessageBoxButton.YesNo,
                                                    MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Employee employee = new Employee();
                if (employee.DeleteEmployee(this, userId))
                {
                    // Refresh the DataGrid
                    employee.LoadEmployees(employeeDataGrid);

                    // Clear selection
                    employeeDataGrid.SelectedItem = null;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Employee employee = new Employee();
            employee.LoadEmployees(employeeDataGrid, txtSearch.Text);
        }


    }

}
