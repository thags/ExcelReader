using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager.StartUpDatabaseOperations(ConfigurationManager.AppSettings.Get("DatabaseName"));
            List<string> allTables = FlowControl.WriteColumnsToDB(ExcelController.Run());
            foreach(string tableName in allTables)
            {
                List<Object[]> columnValues = DBManager.ReadAllFromTable(tableName);
                
            }
            FlowControl.WaitForUser();

        }
    }
}
