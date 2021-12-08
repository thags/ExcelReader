using System;
using System.Configuration;
using System.IO;
using OfficeOpenXml;

namespace ExcelReader
{
    class ExcelController
    {
		public static void Run()
		{
			FileInfo existingFile = new FileInfo(@"C:\Users\Tyler\source\repos\ExcelReader\TestWorkbook.xlsx");
			using (ExcelPackage package = new ExcelPackage(existingFile))
			{
				int NumberOfWorksheets = package.Workbook.Worksheets.Count;
				//iterate through each workbook of the file
				for(int currentWorksheet = 0; currentWorksheet < NumberOfWorksheets; currentWorksheet++)
                {
					ExcelWorksheet worksheet = package.Workbook.Worksheets[currentWorksheet];
					string workSheetName = worksheet.Name;
					DBManager.AddTable(workSheetName);

					//get the dimensions of the table
					int workSheetColumns = worksheet.Dimension.Columns;
					int workSheetRows = worksheet.Dimension.Rows;

					//iterate through each column of the file
					for(int currentColumn = 0; currentColumn <= workSheetColumns; currentColumn++)
                    {
						string indexToLetter = NumberToAlpha(currentColumn);
						DBManager.AddColumn(workSheetName, indexToLetter);
                    }
					Console.WriteLine("\tCell({0},{1}).Value={2}", 1, 1, worksheet.Cells[1, 1].Value);
                    Console.WriteLine($"{workSheetColumns} : {workSheetRows}");
                    Console.WriteLine(workSheetName);
				}
			} // the using statement automatically calls Dispose() which closes the package.
			Console.WriteLine("\n Read workbook complete");
		}

		public static string NumberToAlpha(long number, bool isLower = false)
		{
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
