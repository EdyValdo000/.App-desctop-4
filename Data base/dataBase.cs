using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Controls;

namespace App_desctop_4.Data_base
{
    public class dataBase
    {
        public string connectionString = "Data Source=" + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppDesctop4.db") + ";Version=3;";

        public bool ValidateUserCredentials(string name, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM login WHERE name = @name AND password = @password";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@password", password);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void InsertUser(string name, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO login (name, password) VALUES (@name, @password)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertLampStatus(string date, string lamp1Status, string lamp2Status, string lamp3Status, string lamp4Status)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO lampStatusRegist (date, lamp1, lamp2, lamp3, lamp4) VALUES (@date, @lamp1, @lamp2, @lamp3, @lamp4)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@lamp1", lamp1Status);
                    command.Parameters.AddWithValue("@lamp2", lamp2Status);
                    command.Parameters.AddWithValue("@lamp3", lamp3Status);
                    command.Parameters.AddWithValue("@lamp4", lamp4Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertPoolStatus(string date, string poolStatus, string userChange)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO poolStatusLog (date, poolStatus, userChange) VALUES (@date, @poolStatus, @userChange)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@poolStatus", poolStatus);
                    command.Parameters.AddWithValue("@userChange", userChange);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void LoadLampStatusIntoDataGrid(DataGrid dataGrid)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM lampStatusRegist";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }

        public void LoadPoolStatusIntoDataGrid(DataGrid dataGrid)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM poolStatusLog";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }
    }

}
