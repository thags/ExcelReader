using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Models
{
    public class RowView
    {
        public string ColumnName { get; set; }
        public List<string> RowData {get; set;}
    }
}
