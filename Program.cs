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
            FlowControl.WriteColumnsToDB(ExcelController.GetAllData());
            TableVisualisationEngine.ViewAllTablesFromDB(DBManager.GetAllTablesFromDB());
            FlowControl.WaitForUser();

        }
    }
}
