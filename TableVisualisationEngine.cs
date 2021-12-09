using System.Collections.Generic;
using System;
using ConsoleTableExt;
using ExcelReader.Models;

namespace ExcelReader
{
    class TableVisualisationEngine
    {
        public static void ViewTable(List<String> columns, List<List<object>> tableData)
        {
            if (tableData.Count == 0)
            {
                Console.WriteLine("Currently empty!");
            }
            else
            {
                ConsoleTableBuilder
               .From(tableData)
               .WithColumn(columns)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine(TableAligntment.Left);
            }
            Console.Write("\n");

        }
    }
}


