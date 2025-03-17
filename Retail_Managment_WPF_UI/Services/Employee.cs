using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Retail_Managment_WPF_UI.Services
{
    class Employee
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public Employee() { }

        public Employee(string username, string password, string phone,
                       string email, string role)
        {
            Username = username;
            HashedPassword = HashPassword(password);
            Phone = phone;
            Email = email;
            Role = role;
        }

        public string HashPassword(string plainPassword)
        {
            // Implement actual hashing logic
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool AddEmployee(Window window)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                if (DbConnect.AddEmployee(Username, HashedPassword, Phone, Role, Email, conn))
                {
                    MessageBox.Show(window, "Employee added successfully!","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show(window, "Error adding employee. Please try again.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show(window, "Error connecting to database. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool verifyEnteredData(Window window, ComboBox comboBox)
        {
            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(HashedPassword) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Role))
            {
                MessageBox.Show(window, "Please fill all fields");
                return false;
            }

            if (Role == "Select a role")
            {
                MessageBox.Show(window,"Please Select a Role");
                return false;
            }

            // Validate email format
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show(window, "Please enter a valid email address");
                return false;
            }

            // Validate phone number format
            if (!Regex.IsMatch(Phone, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show(window, "Please enter a valid phone number");
                return false;
            }

            // If all fields pass validation, return true
            return true;
        }

        public void LoadEmployees(DataGrid dataGrid)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                DataTable dt = DbConnect.GetEmployees(conn);
                List<Employee> employees = new List<Employee>();

                foreach (DataRow row in dt.Rows)
                {
                    employees.Add(new Employee(
                        username: row["Username"].ToString(),
                        password: "",
                        phone: row["Phone"].ToString(),
                        email: row["Email"].ToString(),
                        role: row["Role"].ToString()
                    )
                    {
                        UserID = Convert.ToInt32(row["UserID"])
                    });
                }

                dataGrid.ItemsSource = employees;
            }
        }
        public void LoadEmployees(DataGrid dataGrid, string searchQuery = "")
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                DataTable dt = DbConnect.GetEmployees(conn, searchQuery);
                List<Employee> employees = new List<Employee>();

                foreach (DataRow row in dt.Rows)
                {
                    employees.Add(new Employee(
                        username: row["Username"].ToString(),
                        password: "",
                        phone: row["Phone"].ToString(),
                        email: row["Email"].ToString(),
                        role: row["Role"].ToString()
                    )
                    {
                        UserID = Convert.ToInt32(row["UserID"])
                    });
                }

                dataGrid.ItemsSource = employees;
            }
        }

        public static Employee GetEmployeeById(int userId)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                string query = "SELECT UserID, Username, PasswordHash, Email, PhoneNumber, Role FROM Employees WHERE UserID = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Employee employee = new Employee(
                            username: reader["Username"].ToString(),
                            password: "",
                            phone: reader["PhoneNumber"].ToString(),
                            email: reader["Email"].ToString(),
                            role: reader["Role"].ToString())
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            HashedPassword = reader["PasswordHash"].ToString()
                        };
                        return employee;
                    }
                }
            }
            return null;
        }

        public bool UpdateEmployee(Window window)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                try
                {
                    bool success = DbConnect.UpdateEmployee(
                        UserID,
                        Username,
                        Email,
                        Phone,
                        Role,
                        HashedPassword,
                        conn
                    );

                    if (success)
                    {
                        MessageBox.Show(window, "Employee updated successfully!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(window, $"Error updating employee: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
        public bool DeleteEmployee(Window window, int userId)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                try
                {
                    bool success = DbConnect.DeleteEmployee(userId, conn);
                    if (success)
                    {
                        MessageBox.Show(window, "Employee deleted successfully!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(window, $"Database error: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool DeleteEmployees(Window window, List<int> userIds)
        {
            MySqlConnection conn = DbConnect.GetConnection();
            if (conn != null)
            {
                try
                {
                    bool success = DbConnect.DeleteEmployees(userIds, conn);
                    if (success)
                    {
                        MessageBox.Show(window,
                            $"{userIds.Count} employee(s) deleted successfully!",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return true;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(window,
                        $"Database error: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            return false;
        }


    }
}
