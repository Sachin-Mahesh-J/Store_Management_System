using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Retail_Managment_WPF_UI.Services
{
    public static class DbConnect
    {
        private static readonly string connectionString = "server=localhost;port=3306;database=RetailManagementDB;uid=root;password=;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Database Connected Successfully!");
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static MySqlDataReader CheckCredentials(string username, string password, MySqlConnection conn)
        {
            try
            {
                string query = "SELECT * FROM Employees WHERE Username = @username";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string hashedPassword = reader["PasswordHash"].ToString();
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                    if (isPasswordCorrect)
                    {
                        Console.WriteLine("Passwords match");
                        return reader;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Password!");
                        return null;
                    }
                }
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                conn.Close();
                return null;
            }
        }

        public static bool AddEmployee(string username, string password, string phone, string role, string email, MySqlConnection conn)
        {
            try
            {
                string query = "INSERT INTO Employees (Username, PasswordHash, Email, PhoneNumber, Role) VALUES (@username, @password, @email, @phone, @role )";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Employee Added Successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static DataTable GetEmployees(MySqlConnection conn)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT UserID, Username, PhoneNumber AS Phone, Role, Email FROM Employees";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return dt;
        }

        public static DataTable GetEmployees(MySqlConnection conn, string searchQuery = "")
        {
            string query = "SELECT UserID, Username, Email, PhoneNumber AS Phone, Role FROM Employees";

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query += " WHERE Username LIKE @search OR Email LIKE @search OR PhoneNumber LIKE @search OR Role LIKE @search";
            }

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }



        public static bool UpdateEmployee(int userId, string username, string email, string phone,
                                 string role, string passwordHash, MySqlConnection conn)
        {
            try
            {
                string query = @"UPDATE Employees SET 
                        Username = @username,
                        Email = @email,
                        PhoneNumber = @phone,
                        Role = @role,
                        PasswordHash = COALESCE(@passwordHash, PasswordHash)
                        WHERE UserID = @userId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@passwordHash",
                    string.IsNullOrEmpty(passwordHash) ? (object)DBNull.Value : passwordHash);
                cmd.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool DeleteEmployee(int userId, MySqlConnection conn)
        {
            try
            {
                string query = "DELETE FROM Employees WHERE UserID = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteEmployees(List<int> userIds, MySqlConnection conn)
        {
            try
            {
                if (userIds.Count == 0) return false;

                
                string parameters = string.Join(",", userIds.Select((_, i) => $"@id{i}"));
                string query = $"DELETE FROM Employees WHERE UserID IN ({parameters})";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                
                for (int i = 0; i < userIds.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@id{i}", userIds[i]);
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employees: {ex.Message}");
                return false;
            }
        }
    }
}
