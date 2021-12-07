using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ExcelReader
{
    class DBManager
    {
        public static void CreateDatabase()
        {
                string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
                SqlConnection myConn = new SqlConnection(connectionString);

                string str = "CREATE DATABASE ExcelReader";
                SqlCommand myCommand = new SqlCommand(str, myConn);

                myConn.Open();
                myCommand.ExecuteNonQuery();

                Console.WriteLine("DataBase is Created Successfully");
                myConn.Close();
        }

        public static bool DatabaseExists(string DatabaseName)
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
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionStringWithDatabase");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
