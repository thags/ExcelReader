using System.Configuration;


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
