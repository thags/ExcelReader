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
            string lastTableName = "";
            var allColumns = ExcelController.Run();
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
                    }
                }

                //check if the column already exists or not
                if (!DBManager.DoesColumnExist(thisTableName, thisColumnName))
                {
                    DBManager.AddColumn(thisTableName, thisColumnName);
                }




            }
        }
    }
}
