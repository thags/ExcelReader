using System;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using ExcelReader.Models;
using System.Collections.Generic;

namespace ExcelReader
{
    class ExcelController
    {
		public static List<Column> Run()
		{
			//create an empty list of columns which will be added to and returned
			List<Column> allColumns = new List<Column>();
			string filePath = ConfigurationManager.AppSettings.Get("FilePath");
			FileInfo existingFile = new FileInfo(filePath);
			using (ExcelPackage package = new ExcelPackage(existingFile))
			{
				int NumberOfWorksheets = package.Workbook.Worksheets.Count;
				//iterate through each workbook of the file
				for(int currentWorksheet = 0; currentWorksheet < NumberOfWorksheets; currentWorksheet++)
                {
					ExcelWorksheet worksheet = package.Workbook.Worksheets[currentWorksheet];
					string workSheetName = worksheet.Name;

					//get the dimensions of the table
					int workSheetColumns = worksheet.Dimension.Columns;
					int workSheetRows = worksheet.Dimension.Rows;

					//iterate through each column of the file
					for(int currentColumn = 1; currentColumn <= workSheetColumns; currentColumn++)
                    {
						string colIndexToLetter = NumberToAlpha(currentColumn);
						List<string> entireColumnData = GetAllDataOfRow(workSheetRows, currentColumn, worksheet);
						
						//create a new column model to later send to DBmanager to create the column
						Column newColumn = new Column
						{
							TableName = workSheetName,
							ColumnName = colIndexToLetter,
							ColumnData = entireColumnData,
						};
						allColumns.Add(newColumn);
						
					}
				}
			} // the using statement automatically calls Dispose() which closes the package.
			return allColumns;
		}

		public static List<string> GetAllDataOfRow(int workSheetRows, int currentColumn, ExcelWorksheet worksheet)
        {
			List<string> entireColumnData = new List<string>(workSheetRows);
			//iterate through all the rows to get row data
			for (int currentRow = 1; currentRow <= workSheetRows; currentRow++)
			{
				string currentValue = (string)worksheet.Cells[currentRow, currentColumn].Value;
				entireColumnData.Add(currentValue);
			}
			return entireColumnData;
		}

		public static string NumberToAlpha(long number, bool isLower = false)
		{
			number--;
			string returnVal = "";
			char c = isLower ? 'a' : 'A';
			while (number >= 0)
			{
				returnVal = (char)(c + number % 26) + returnVal;
				number /= 26;
				number--;
			}

			return returnVal;
		}
	}
}
