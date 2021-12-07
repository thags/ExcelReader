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
				for(int i = 0; i < NumberOfWorksheets; i++)
                {
					ExcelWorksheet worksheet = package.Workbook.Worksheets[i];
					int workSheetColumns = worksheet.Dimension.Columns;
					int workSheetRows = worksheet.Dimension.Rows;
					string workSheetName = worksheet.Name;
					Console.WriteLine("\tCell({0},{1}).Value={2}", 1, 1, worksheet.Cells[1, 1].Value);
                    Console.WriteLine($"{workSheetColumns} : {workSheetRows}");
                    Console.WriteLine(workSheetName);
				}



			} // the using statement automatically calls Dispose() which closes the package.

			Console.WriteLine();
			Console.WriteLine("Read workbook sample complete");
			Console.WriteLine();
		}
	}
}
