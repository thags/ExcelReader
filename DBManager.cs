using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

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
            string dropDBString = $@"EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'{DatabaseName}'
                                    USE [master]
                                    DROP DATABASE [{DatabaseName}]";
            SqlCommand dropDBCommand = new SqlCommand(dropDBString, con);
            dropDBCommand.ExecuteNonQuery();
            con.Close();
        }
        public static void AddTable(string tableName)
        {
            SqlConnection con = OpenSql();
            string command = $@"CREATE TABLE [dbo].[{tableName}](
                                [Id][int] IDENTITY(1, 1) NOT NULL)";

            SqlCommand myCommand = new SqlCommand(command, con);
            myCommand.ExecuteNonQuery();
            con.Close();
        }
        public static void AddColumn(string tableName, string columnName)
        {
            SqlConnection con = OpenSql();
            string command = $"ALTER TABLE [dbo].[{tableName}] ADD {columnName} VARCHAR(255)";

            SqlCommand myCommand = new SqlCommand(command, con);
            myCommand.ExecuteNonQuery();
            con.Close();
        }
        public static void UpdateColumns(string tableName, string columnName, string data, int index)
        {
            SqlConnection con = OpenSql();

            string command = $@"UPDATE {tableName}
                             SET {columnName} = '{data}'
                             WHERE Id = {index}";

            SqlCommand myCommand = new SqlCommand(command, con);
            myCommand.ExecuteNonQuery();
            con.Close();
        }
        public static void InsertToColumn(string tableName, string columnName, string data)
        {
            SqlConnection con = OpenSql();

            string command = $"INSERT INTO {tableName}({columnName}) VALUES ('{data}')";

            SqlCommand myCommand = new SqlCommand(command, con);
            myCommand.ExecuteNonQuery();
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
        public static bool DoesTableExist(string tableName)
        {
            bool isExist = false;
            SqlConnection con = OpenSql();
            string command = $@"SELECT *
                                FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_NAME = N'{tableName}'";
            using (SqlCommand cmd = new SqlCommand(command, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    isExist = reader.HasRows;
                }
            }
            con.Close();
            return isExist;
        }
        public static bool DoesColumnExist(string tableName, string columnName)
        {
            bool isExist = false;
            SqlConnection con = OpenSql();
            string command = $@"SELECT * 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE table_name = '{tableName}'
                                AND column_name = '{columnName}'";
            using (SqlCommand cmd = new SqlCommand(command, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    isExist = reader.HasRows;
                }
            }
            con.Close();
            return isExist;
        }
        public static List<Object[]> ReadAllFromTable(string tableName)
        {
            
            List<Object[]> allColumnValues = new List<object[]>();
            string command = $"SELECT * FROM {tableName}";

            SqlConnection con = OpenSql();
            SqlCommand myCommand = new SqlCommand(command, con);
            SqlDataReader dataReader = myCommand.ExecuteReader();
            
            while (dataReader.Read())
            {
                int numberOfFields = dataReader.FieldCount;
                Object[] columnValues = new Object[numberOfFields];
                dataReader.GetValues(columnValues);
                allColumnValues.Add(columnValues);

            }
            con.Close();
            return allColumnValues;
        }
        public static SqlConnection OpenSql()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString") + $"Initial Catalog={ConfigurationManager.AppSettings.Get("DatabaseName")}";
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
