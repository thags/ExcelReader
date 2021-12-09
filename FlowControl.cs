using ExcelReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader
{
    public class FlowControl
    {
        public static List<string> WriteColumnsToDB(List<Column> allColumns)
        {
            List<string> allTables = new List<string>();
            string lastTableName = "";
            int iterations = 0;
            foreach (Column column in allColumns)
            {
                string thisTableName = column.TableName;
                string thisColumnName = column.ColumnName;

                //See if this current iteration is part of a different table(workbook)
                if (lastTableName != thisTableName)
                {
                    //Check if the table exists or not, create it if it does not
                    if (!DBManager.DoesTableExist(thisTableName))
                    {
                        DBManager.AddTable(thisTableName);
                        allTables.Add(thisTableName);
                        Console.WriteLine($"Adding Database table {thisTableName}");
                    }
                }
                lastTableName = thisTableName;

                //check if the column already exists or not
                if (!DBManager.DoesColumnExist(thisTableName, thisColumnName))
                {
                    DBManager.AddColumn(thisTableName, thisColumnName);
                    Console.WriteLine($"\nAdding column {thisColumnName} to table {thisTableName}");
                }

                int index = 1;
                Console.WriteLine($"Inserting data into column: {thisColumnName} in table {thisTableName}");
                foreach (string data in column.ColumnData)
                {
                    if (iterations == 0)
                    {
                        DBManager.InsertToColumn(thisTableName, thisColumnName, data);
                    }
                    else
                    {
                        //update the columns instead of inserting
                        DBManager.UpdateColumns(thisTableName, thisColumnName, data, index);
                    }
                    index++;
                }
                Console.WriteLine($"Data insertion complete for {thisColumnName}");

                iterations++;
            }
            return allTables;
        }
        public static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
    }
}
