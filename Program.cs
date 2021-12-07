using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!DBManager.DatabaseExists("ExcelReader"))
            {
                Console.WriteLine("Database does not exist");
                DBManager.CreateDatabase();
            }
            else { Console.WriteLine("Database already exists"); }
        }
    }
}
