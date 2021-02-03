using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace UnitTestProject1
{
    [TestClass]
    public class WorkWithExcelTest
    {
        [TestMethod]
        public void Table_To_ScheduleRowObject_Test()
        {
            //Create a test file
            var fi = new FileInfo(@"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\work.xlsx");

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();
                var scheduleRowObjects = worksheet.Tables.First().ConvertTableToObjects<ScheduleRowObject>().ToList();
                foreach (var data in scheduleRowObjects)
                {
                    Console.WriteLine(data.FlightDate + ":" + data.AirCompanyName + ":" + data.ParkingSector);
                }

                package.Save();
            }
        }

       
    }
}
