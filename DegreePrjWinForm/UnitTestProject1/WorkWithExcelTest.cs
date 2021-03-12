using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

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

        [TestMethod]
        public void ReadXml()
        {
            var _planeParkingObjects = new List<PlaneParkingObject>();
            var pp = new PlaneParkingObject {Id = "1", Number = "1A"};
            _planeParkingObjects.Add(pp);

            var pathToFile = @"D:\chetv_va\Диплом 2021\Данные для работы\Xml\";
            foreach (var o in _planeParkingObjects)
            {
                o.Coordinates = new List<CoordinateObject>();
                var path = pathToFile + o.Number + ".xml";
                XDocument xdoc = XDocument.Load(path);

                XElement geozoneType = xdoc.Element("geozoneType");

                XElement geometry = geozoneType.Elements("geometry").FirstOrDefault();
                foreach (XElement phoneElement in geometry.Elements("point"))
                {
                    XAttribute nameX = phoneElement.Attribute("x");
                    XAttribute nameY = phoneElement.Attribute("y");
                    if (nameX != null && nameY != null)
                    {
                        var coordObj = new CoordinateObject();
                        var englishCulture = CultureInfo.GetCultureInfo("en-US");
                        coordObj.X = double.Parse(nameX.Value, englishCulture);
                        coordObj.Y = double.Parse(nameY.Value, englishCulture);
                        o.Coordinates.Add(coordObj);
                    }
                }
            }
        }

       
    }
}
