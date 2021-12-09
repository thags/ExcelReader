using ExcelReader.Models;
using System;
using System.Collections.Generic;

namespace ExcelReader
{
    public class FlowControl
    {
        public static void WriteColumnsToDB(List<Column> allColumns)
        {
            int iterations = 0;
            string lastTableName = "";
            foreach (Column column in allColumns)
            {
                string thisTableName = column.TableName;
                string thisColumnName = column.ColumnName;

                if(lastTableName != thisTableName)
                {
                    iterations = 0;
                }

                AddTableToDBIfNonExist(thisTableName);
                AddColumnToDBTableIfNonExist(thisTableName, thisColumnName);
                AddColumnDataToDB(column, iterations, thisTableName);

                lastTableName = thisTableName;
                iterations++;
            }
        }

        private static void AddColumnDataToDB(Column column, int iterations, string tableName)
        {
            Console.WriteLine($"Inserting data into column: {column.ColumnName} in table {tableName}");
            int index = 1;
            foreach (string data in column.ColumnData)
            {
                if (iterations == 0)
                {
                    DBManager.InsertToColumn(tableName, column.ColumnName, data);
                }
                else
                {
                    //update the columns instead of inserting
                    DBManager.UpdateColumns(tableName, column.ColumnName, data, index);
                }
                index++;
            }
            Console.WriteLine($"Data insertion complete for {column.ColumnName}");
        }
        private static void AddTableToDBIfNonExist(string tableName)
        {
            //Check if the table exists or not, create it if it does not
            if (!DBManager.DoesTableExist(tableName))
            {
                DBManager.AddTable(tableName);
                Console.WriteLine($"Adding Database table {tableName}");
            }
        }

        private static void AddColumnToDBTableIfNonExist(string tableName, string columnName)
        {
            //check if the column already exists or not
            if (!DBManager.DoesColumnExist(tableName, columnName))
            {
                DBManager.AddColumn(tableName, columnName);
                Console.WriteLine($"\nAdding column {columnName} to table {tableName}");
            }
        }
        public static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
    }
}