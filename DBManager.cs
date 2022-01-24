using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace ExcelReader
{
    class DBManager
    {
        private static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

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
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"CREATE DATABASE {DatabaseName}";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteDatabase(string DatabaseName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'{DatabaseName}'
                                    USE [master]
                                    DROP DATABASE [{DatabaseName}]";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void AddTable(string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"CREATE TABLE [dbo].[{tableName}](
                                [Id][int] IDENTITY(1, 1) NOT NULL)";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void AddColumn(string tableName, string columnName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"ALTER TABLE [dbo].[{tableName}] ADD {columnName} VARCHAR(255)";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateColumns(string tableName, string columnName, string data, int index)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"UPDATE {tableName}
                             SET {columnName} = '{data}'
                             WHERE Id = {index}";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertToColumn(string tableName, string columnName, string data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"INSERT INTO {tableName}({columnName}) VALUES ('{data}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static bool DoesDatabaseExist(string DatabaseName)
        {
            bool isExist = false;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $"SELECT * FROM master.dbo.sysdatabases WHERE name = '{DatabaseName}'";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                }
            }

            return isExist;
        }
        public static bool DoesTableExist(string tableName)
        {
            bool isExist = false;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"SELECT *
                                FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_NAME = N'{tableName}'";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                }
            }
            return isExist;
        }
        public static bool DoesColumnExist(string tableName, string columnName)
        {
            bool isExist = false;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = $@"SELECT * 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE table_name = '{tableName}'
                                AND column_name = '{columnName}'";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        isExist = reader.HasRows;
                    }
                }
            }
            return isExist;

        }
        public static List<List<object>> ReadAllRowsFromTable(string tableName)
        {
            List<List<object>> allColumnsData = new List<List<object>>();
            int iteration = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"SELECT * FROM {tableName}";
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int numberOfFields = dataReader.FieldCount;

                            List<object> currentColumnData = new List<object>(numberOfFields);
                            for (int i = 0; i < numberOfFields; i++)
                            {
                                currentColumnData.Add(dataReader[i].ToString());
                            }
                            allColumnsData.Add(currentColumnData);
                            iteration++;
                        }
                    }
                }
            }
            
            return allColumnsData;
        }
        public static List<string> GetAllColumnHeadingsFromTable(string tableName)
        {
            List<string> allHeadings = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $@"SELECT COLUMN_NAME
                               FROM INFORMATION_SCHEMA.COLUMNS
                                WHERE TABLE_NAME = N'{tableName}'";
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string currentHeading = dataReader[0].ToString();
                            if (currentHeading == "Id")
                            {
                                currentHeading = "";
                            }
                            allHeadings.Add(currentHeading);
                        }
                    }
                }
            }
            return allHeadings;
        }
        public static List<string> GetAllTablesFromDB()
        {
            List<string> allTables = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $@"SELECT TABLE_NAME
                               FROM INFORMATION_SCHEMA.TABLES";
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string currentHeading = dataReader[0].ToString();
                            if (currentHeading == "Id")
                            {
                                currentHeading = "";
                            }
                            allTables.Add(currentHeading);
                        }
                    }
                }
            }

            return allTables;
        }
    }
}
