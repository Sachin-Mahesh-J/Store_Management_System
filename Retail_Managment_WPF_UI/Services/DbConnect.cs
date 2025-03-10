using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                string query = "SELECT * FROM Users WHERE Username = @username AND PasswordHash = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                conn.Close();
                return null;
            }
        }
    }
}
