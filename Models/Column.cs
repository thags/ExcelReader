using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Models
{
    public class Column
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public List<string> ColumnData {get; set;}
    }
}
