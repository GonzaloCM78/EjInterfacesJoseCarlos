using System;
using MySql.Data.MySqlClient;

namespace EjInterfacesJoseCarlos
{
    class Database
    {
        private string connectionString = "server=localhost;database=northwind;user=root;password=root;";
        private MySqlConnection connection;

        public Database()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
