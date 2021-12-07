using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ExcelReader
{
    class DBManager
    {
        public static void StartUpDatabaseOperations(string DatabaseName)
        {
            if (!DoesDatabaseExist(DatabaseName))
            {
                Console.WriteLine("Database does not exist");
                Console.WriteLine("Creating database...");
                CreateDatabase("ExcelReader");
                Console.WriteLine("Database Created");
            }
            else 
            { 
                Console.WriteLine("Database already exists");
                Console.WriteLine("Deleting previous database...");
                DeleteDatabase(DatabaseName);
                Console.WriteLine("Previous database deleted");
                Console.WriteLine("Creating new database...");
                CreateDatabase(DatabaseName);
                Console.WriteLine("New database created");
            }
        }
        public static void CreateDatabase(string DatabaseName)
        {
                string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
                SqlConnection myConn = new SqlConnection(connectionString);

                string str = $"CREATE DATABASE {DatabaseName}";
                SqlCommand myCommand = new SqlCommand(str, myConn);

                myConn.Open();
                myCommand.ExecuteNonQuery();

                myConn.Close();
        }
        public static void DeleteDatabase(string DatabaseName)
        {
            string conString = ConfigurationManager.AppSettings.Get("ConnectionString");
            var con = new SqlConnection(conString);
            con.Open();
            string dropDBString = $"DROP DATABASE {DatabaseName}";
            SqlCommand dropDBCommand = new SqlCommand(dropDBString, con);
            dropDBCommand.ExecuteNonQuery();
            con.Close();
        }

        public static bool DoesDatabaseExist(string DatabaseName)
        {
            string cmdText = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{DatabaseName}'";
            bool isExist = false;
            string conString = ConfigurationManager.AppSettings.Get("ConnectionString");
            var con = new SqlConnection(conString);
            con.Open();
            using (SqlCommand cmd = new SqlCommand(cmdText, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    isExist = reader.HasRows;
                }
            }
            con.Close();

            return isExist;
        }
        public static SqlConnection OpenSql()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") + $"Initial Catalog='{ConfigurationManager.AppSettings.Get("DatabaseName")}'";
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
