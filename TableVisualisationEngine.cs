using System.Collections.Generic;
using System;
using ConsoleTableExt;
using ExcelReader.Models;

namespace ExcelReader
{
    class TableVisualisationEngine
    {
        public static void ViewTable(List<String> columns, List<List<object>> tableData, string tableName)
        {
            if (tableData.Count == 0)
            {
                Console.WriteLine("Currently empty!");
            }
            else
            {
                ConsoleTableBuilder
               .From(tableData)
               .WithTitle(tableName)
               .WithColumn(columns)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine(TableAligntment.Left);
            }
            Console.Write("\n");
        }

        public static void ViewAllTablesFromDB(List<string> tableNames)
        {
            if (tableNames.Count == 0)
            {
                Console.WriteLine("Currently empty!");
            }
            else
            {
                foreach(string tableName in tableNames)
                {
                    var columns = (DBManager.GetAllColumnHeadingsFromTable(tableName));
                    var tableData = DBManager.ReadAllRowsFromTable(tableName);
                    ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle(tableName)
                    .WithColumn(columns)
                    .WithFormat(ConsoleTableBuilderFormat.Alternative)
                    .ExportAndWriteLine(TableAligntment.Left);
                }
                
            }
        }
    }
}


