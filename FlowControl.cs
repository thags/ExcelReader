using ExcelReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader
{
    class FlowControl
    {
        public void WriteAllExcelToDB()
        {
            var allColumns = ExcelController.Run();
            foreach (Column column in allColumns)
            {

            }
        }
    }
}
