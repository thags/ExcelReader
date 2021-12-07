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

			FileInfo existingFile = new FileInfo("D:\\Downloads\\time_week01.xlsx");
			using (ExcelPackage package = new ExcelPackage(existingFile))
			{
				//Get the first worksheet in the workbook
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
				Console.WriteLine("\tCell({0},{1}).Value={2}", 1, 1, worksheet.Cells[1, 1].Value);


			} // the using statement automatically calls Dispose() which closes the package.

			Console.WriteLine();
			Console.WriteLine("Read workbook sample complete");
			Console.WriteLine();
		}
	}
}
