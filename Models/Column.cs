using System.Collections.Generic;

namespace ExcelReader.Models
{
    public class Column
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public List<string> ColumnData {get; set;}
    }
}
