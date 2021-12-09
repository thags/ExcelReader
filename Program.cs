using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ExcelReader.Models;

namespace ExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager.StartUpDatabaseOperations(ConfigurationManager.AppSettings.Get("DatabaseName"));
            List<string> allTables = FlowControl.WriteColumnsToDB(ExcelController.Run());
            List<RowView> rowValues = new List<RowView>();
            var headings = (DBManager.GetAllColumnHeadingsFromTable("Sheet1"));
            TableVisualisationEngine.ViewTable(headings, DBManager.ReadAllRowsFromTable("Sheet1"));

            FlowControl.WaitForUser();

        }
    }
}
