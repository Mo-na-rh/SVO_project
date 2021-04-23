using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Managers;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис по составлению результирующего отчёта
    /// </summary>
    public static class ReportService
    {
        /// <summary>
        /// Компоновка и запись в файл результирующего отчёта
        /// </summary>
        /// <param name="pathToResultReportFile"></param>
        /// <param name="objectManager"></param>
        public static void WriteTestResultReport(string pathToResultReportFile, ObjectManager objectManager)
        {
            var fi = new FileInfo(pathToResultReportFile);
            using (TextWriter tw = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                WriteFlights(tw, objectManager.ScheduleRows);
                WriteParkings(tw, objectManager.Parkings);
                WriteAircrafts(tw, objectManager.Aircrafts);
                WriteEmptyParkings(tw, objectManager.ParkingBlocks);
            }
        }

        /// <summary>
        /// Запись рейсов
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="_scheduleRowObjects"></param>
        private static void WriteFlights(TextWriter tw, List<ScheduleRow> _scheduleRowObjects)
        {
            tw.WriteLine(" Flights");
            tw.WriteLine(" ============================================================================================");
            var i = 0;
            foreach (var row in _scheduleRowObjects)
            {
                tw.WriteLine(
                    $"num: {i} / FlightDate: {DateTime.Parse(row.FlightDate).ToShortDateString()} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToShortTimeString()} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                i++;
            }
        }

        /// <summary>
        /// Запись мест стоянок
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="parkings"></param>
        private static void WriteParkings(TextWriter tw, List<Parking> parkings)
        {
            tw.WriteLine(" ============================================================================================");
            tw.WriteLine(" Plane parkings");
            tw.WriteLine(" ============================================================================================");
            foreach (var parking in parkings)
            {
                tw.WriteLine($"num: {parking.Id} / Number: {parking.Number} / MiddleX: {parking.MiddleX()} / MiddleY: {parking.MiddleY()}");
                foreach (var row in parking.LinkedScheduleRows)
                {
                    tw.WriteLine(
                        $"  FlightDate: {DateTime.Parse(row.FlightDate).ToShortDateString()} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToShortTimeString()} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                }
            }
        }

        /// <summary>
        /// Запись типов ВС
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="aircrafts"></param>
        private static void WriteAircrafts(TextWriter tw, List<Aircraft> aircrafts)
        {
            tw.WriteLine(" ============================================================================================");
            tw.WriteLine(" Aircrafts");
            tw.WriteLine(" ============================================================================================");
            foreach (var row in aircrafts)
            {
                tw.WriteLine($"num: {row.Id} / IATA: {row.IATA} / ICAO: {row.ICAO} / Rus: {row.RUS} / Name: {row.Name} ");
            }
        }

        /// <summary>
        /// Вывод пустых блоков МС
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="_parkingBlocks"></param>
        private static void WriteEmptyParkings(TextWriter tw, List<ParkingBlock> _parkingBlocks)
        {
            tw.WriteLine(" ============================================================================================");
            tw.WriteLine(" Empty parkings");
            tw.WriteLine(" ============================================================================================");
            foreach (var row in _parkingBlocks.Where(t => !t.IsFilled).ToList())
            {
                tw.WriteLine($"id block: {row.Id}");
                foreach (var parking in row.Parkings)
                {
                    tw.WriteLine($" num: {parking.Id} / Number: {parking.Number} ");
                }
            }
        }

        /// <summary>
        /// Компоновка и запись в файл результирующего отчёта
        /// </summary>
        /// <param name="pathToResultReportFile"></param>
        /// <param name="objectManager"></param>
        public static void WriteResultReportTxt(string pathToResultReportFile, ObjectManager oM)
        {
            var fi = new FileInfo(pathToResultReportFile);
            using (TextWriter tw = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                tw.WriteLine("Отчёт по расчету минимального количества СНО.");
                tw.WriteLine(string.Empty);
                tw.WriteLine($"На даты с {oM.FromDate} по {oM.ToDate}");
                tw.WriteLine(string.Empty);
                tw.WriteLine("Сформирована следующая разбивка по блокам: ");
                tw.WriteLine(string.Empty);
                foreach (var pb in oM.ParkingBlocks)
                {
                    tw.WriteLine($" Блок {pb.Id}");
                    foreach (var parking in pb.Parkings)
                    {
                        tw.WriteLine($"     - Место стоянки {parking.Number}");
                    }
                    tw.WriteLine($" Требуемое количество СНО на блок по типам: стремянки {pb.GetGseCountByType(GseType.ladder)}, " +
                                 $"упорные колодки {pb.GetGseCountByType(GseType.block)}, " +
                                 $"конуса безопасности {pb.GetGseCountByType(GseType.markerCone)}, " +
                                 $"буксировочные водила {pb.GetGseCountByType(GseType.towhead)}.");
                }

                tw.WriteLine($"Итоговое количество блоков: {oM.ParkingBlocks.Count()}");
                tw.WriteLine($"Общее потребное количество СНО по типам: стремянки {oM.GetGseCountByType(GseType.ladder)}, " +
                             $"упорные колодки {oM.GetGseCountByType(GseType.block)}, " +
                             $"конуса безопасности {oM.GetGseCountByType(GseType.markerCone)}, " +
                             $"буксировочные водила {oM.GetGseCountByType(GseType.towhead)}.");
            }
        }

        /// <summary>
        /// Компоновка и запись в файл результирующего отчёта
        /// </summary>
        /// <param name="pathToResultReportFile"></param>
        /// <param name="objectManager"></param>
        public static void WriteResultReportExcel(string pathToResultReportFile, ObjectManager oM)
        {
            //var fi = new FileInfo(pathToResultReportFile);
            //using (TextWriter tw = new StreamWriter(fi.Open(FileMode.Truncate)))
            //{
            //    tw.WriteLine("Отчёт по расчету минимального количества СНО.");
            //    tw.WriteLine(string.Empty);
            //    tw.WriteLine($"На даты с {oM.FromDate} по {oM.ToDate}");
            //    tw.WriteLine(string.Empty);
            //    tw.WriteLine("Сформирована следующая разбивка по блокам: ");
            //    tw.WriteLine(string.Empty);
            //    foreach (var pb in oM.ParkingBlocks)
            //    {
            //        tw.WriteLine($" Блок {pb.Id}");
            //        foreach (var parking in pb.AircraftParkings)
            //        {
            //            tw.WriteLine($"     - Место стоянки {parking.Number}");
            //        }
            //        tw.WriteLine($" Требуемое количество СНО на блок по типам: стремянки {pb.GetGseCountByType(GseType.ladder)}, " +
            //                     $"упорные колодки {pb.GetGseCountByType(GseType.block)}, " +
            //                     $"конуса безопасности {pb.GetGseCountByType(GseType.markerCone)}, " +
            //                     $"буксировочные водила {pb.GetGseCountByType(GseType.towhead)}.");
            //    }

            //    tw.WriteLine($"Итоговое количество блоков: {oM.ParkingBlocks.Count()}");
            //    tw.WriteLine($"Общее потребное количество СНО по типам: стремянки {oM.GetGseCountByType(GseType.ladder)}, " +
            //                 $"упорные колодки {oM.GetGseCountByType(GseType.block)}, " +
            //                 $"конуса безопасности {oM.GetGseCountByType(GseType.markerCone)}, " +
            //                 $"буксировочные водила {oM.GetGseCountByType(GseType.towhead)}.");
            //}
        }

        public static void TestWritingInExcel()
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var Articles = new[]
            {
                new {
                    Id = "101", Name = "C++"
                },
                new {
                    Id = "102", Name = "Python"
                },
                new {
                    Id = "103", Name = "Java Script"
                },
                new {
                    Id = "104", Name = "GO"
                },
                new {
                    Id = "105", Name = "Java"
                },
                new {
                    Id = "106", Name = "C#"
                }
            };

            // Creating an instance
            // of ExcelPackage
            ExcelPackage excel = new ExcelPackage();

            // name of the sheet
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            // setting the properties
            // of the work sheet 
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties
            // of the first row
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            // Header of the Excel sheet
            workSheet.Cells[1, 1].Value = "S.No";
            workSheet.Cells[1, 2].Value = "Id";
            workSheet.Cells[1, 3].Value = "Name";

            // Inserting the article data into excel
            // sheet by using the for each loop
            // As we have values to the first row 
            // we will start with second row
            int recordIndex = 2;

            foreach (var article in Articles)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = article.Id;
                workSheet.Cells[recordIndex, 3].Value = article.Name;
                recordIndex++;
            }

            // By default, the column width is not 
            // set to auto fit for the content
            // of the range, so we are using
            // AutoFit() method here. 
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();

            // file name with .xlsx extension 
            string p_strPath = GetPathToExcel() + "report.xlsx";

            if (File.Exists(p_strPath))
                File.Delete(p_strPath);

            // Create excel file on physical disk 
            FileStream objFileStrm = File.Create(p_strPath);
            objFileStrm.Close();

            // Write content to excel file 
            File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
            //Close Excel package
            excel.Dispose();
        }

        /// <summary>
        /// Получение пути к текущей папке с Xml
        /// </summary>
        /// <returns></returns>
        public static string GetPathToExcel()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.IndexOf("bin"));
            return path + @"Source\";
        }

    }
}
