using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BookShopManagement
{
        public class Connect
        {
            private SqlConnection connection;
            private string connectionString = @"Data Source=DESKTOP-B55MLD8\SQLEXPRESS;Initial Catalog=reviews2;Integrated Security=True";

        public Connect()
            {
                connection = new SqlConnection(connectionString);
            }

            public SqlConnection GetConnection()
            {
                return connection;
            }

            public void OpenConnection()
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }

            public void CloseConnection()
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }
 }
