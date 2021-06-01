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
        #region Excel

        /// <summary>
        /// Вывод отчёта в файл Excel
        /// </summary>
        /// <param name="objectManager"></param>
        public static void WriteReportInExcel(ObjectManager objectManager)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Creating an instance of ExcelPackage
            var package = new ExcelPackage();

            // add sheet
            var sheet = package.Workbook.Worksheets.Add("Отчёт");

            AddReportHeader(sheet, objectManager.FromDate, objectManager.ToDate);

            AddTableHeader(sheet);

            var row = 5;

            row = AddReportBody(objectManager, sheet, row);

            row++;

            AddReportSummary(objectManager, sheet, row);

            SetStyles(sheet, row);

            var fileName = objectManager.GetReportFileName();

            SaveInExcel(package, fileName);

            //Close Excel package
            package.Dispose();
        }

        /// <summary>
        /// Форматирование и настройка параметров листа Excel
        /// </summary>
        /// <param name="sheet">Рабочая страница</param>
        /// <param name="row">Индекс строки</param>
        private static void SetStyles(ExcelWorksheet sheet, int row)
        {
            // Setting the properties of the header table row
            sheet.Row(4).Height = 20;
            sheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // покраска шапки
            sheet.Cells[4, 1, 4, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[4, 1, 4, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            sheet.Cells[4, 1, 4, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Column(2).Width = 15;
            sheet.Column(3).AutoFit();
            sheet.Column(4).AutoFit();
        }

        /// <summary>
        /// Заголовок отчёта
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        private static void AddReportHeader(ExcelWorksheet sheet, DateTime fromDate, DateTime toDate)
        {
            sheet.Cells[2, 1].Value = $"Прогноз потребности в СНО на период с 00:00 {fromDate.ToShortDateString()} по 23:59 {toDate.ToShortDateString()}";
        }

        /// <summary>
        /// Шапка таблицы отчёта
        /// </summary>
        /// <param name="sheet"></param>
        private static void AddTableHeader(ExcelWorksheet sheet)
        {
            // Header of the Excel sheet
            sheet.Cells[4, 1].Value = "Блок";
            sheet.Cells[4, 2].Value = "МС";
            sheet.Cells[4, 3].Value = "Тип СНО";
            sheet.Cells[4, 4].Value = "Количество(шт)";
        }

        /// <summary>
        /// Отрисовка тела отчёта
        /// </summary>
        /// <param name="objectManager"></param>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static int AddReportBody(ObjectManager objectManager, ExcelWorksheet sheet, int row)
        {
            var startRow = row;
            // TODO КАждый блок тонким контуром
            foreach (var block in objectManager.ParkingBlocks)
            {
                sheet.Cells[row, 1].Value = block.Id;
                sheet.Cells[row, 2].Value = GetParkingsByComma(block);

                sheet.Cells[row, 1, row + 3, 1].Merge = true;
                sheet.Cells[row, 2, row + 3, 2].Merge = true;

                // Выравнивание
                sheet.Cells[row, 1, row + 3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 1, row + 3, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sheet.Cells[row, 1, row + 3, 2].AutoFitColumns();

                for (int i = 1; i < 5; i++)
                {
                    var typeGse = "";
                    var countGse = 0;
                    if (i == 1)
                    {
                        typeGse = "стремянки";
                        countGse = block.LadderGseCount;
                    }
                    if (i == 2)
                    {
                        typeGse = "упорные колодки";
                        countGse = block.BlockGseCount;
                    }
                    if (i == 3)
                    {
                        typeGse = "конуса безопасности";
                        countGse = block.MarkerConeGseCount;
                    }
                    if (i == 4)
                    {
                        typeGse = "буксировочные водила";
                        countGse = block.TowBarGseCount;
                    }

                    sheet.Cells[row, 3].Value = typeGse;
                    sheet.Cells[row, 4].Value = countGse;
                    row++;
                }
            }

            sheet.Cells[startRow, 1, row - 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            return row;
        }

        /// <summary>
        /// Вывод результатов отчёта
        /// </summary>
        /// <param name="objectManager"></param>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        private static void AddReportSummary(ObjectManager objectManager, ExcelWorksheet sheet, int row)
        {
            var startIndex = row;
            sheet.Cells[row, 1].Value = "Всего  на период";
            sheet.Cells[row, 1, row, 2].Merge = true;
            sheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            for (int i = 1; i < 5; i++)
            {
                var typeGse = "";
                var countGse = 0;
                if (i == 1)
                {
                    typeGse = "стремянки";
                    countGse = objectManager.GetGseCountByType(GseType.ladder);
                }
                if (i == 2)
                {
                    typeGse = "упорные колодки";
                    countGse = objectManager.GetGseCountByType(GseType.block);
                }
                if (i == 3)
                {
                    typeGse = "конуса безопасности";
                    countGse = objectManager.GetGseCountByType(GseType.markerCone);
                }
                if (i == 4)
                {
                    typeGse = "буксировочные водила";
                    countGse = objectManager.GetGseCountByType(GseType.towBar);
                }

                sheet.Cells[row, 3].Value = typeGse;
                sheet.Cells[row, 4].Value = countGse;
                row++;
            }

            var usedRange = sheet.Cells[startIndex, 1, row - 1, 4];
            usedRange.Style.Border.BorderAround(ExcelBorderStyle.Thick);
        }

        /// <summary>
        /// Сохранение объекта в excel файл
        /// </summary>
        /// <param name="excel"></param>
        private static void SaveInExcel(ExcelPackage excel, string fileName)
        {
            // file name with .xlsx extension 
            var fileReportPath = GetPathToExcel() + fileName +".xlsx";

            if (File.Exists(fileReportPath))
                File.Delete(fileReportPath);

            // Create excel file on physical disk 
            var objFileStream = File.Create(fileReportPath);
            objFileStream.Close();

            // Write content to excel file 
            File.WriteAllBytes(fileReportPath, excel.GetAsByteArray());
        }

        /// <summary>
        /// Получение пути к расположению отчёта
        /// </summary>
        /// <returns></returns>
        public static string GetPathToExcel()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.IndexOf("bin"));
            return path + @"Source\";
        }
        
        #endregion

        #region Txt

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

        ///// <summary>
        ///// Компоновка и запись в файл результирующего отчёта
        ///// </summary>
        ///// <param name="pathToResultReportFile"></param>
        ///// <param name="objectManager"></param>
        //public static void WriteResultReportTxt(string pathToResultReportFile, ObjectManager oM)
        //{
        //    var fi = new FileInfo(pathToResultReportFile);
        //    using (TextWriter tw = new StreamWriter(fi.Open(FileMode.Truncate)))
        //    {
        //        tw.WriteLine("Отчёт по расчету минимального количества СНО.");
        //        tw.WriteLine(string.Empty);
        //        tw.WriteLine($"На даты с {oM.FromDate} по {oM.ToDate}");
        //        tw.WriteLine(string.Empty);
        //        tw.WriteLine("Сформирована следующая разбивка по блокам: ");
        //        tw.WriteLine(string.Empty);
        //        foreach (var pb in oM.ParkingBlocks)
        //        {
        //            tw.WriteLine($" Блок {pb.Id}");
        //            foreach (var parking in pb.Parkings)
        //            {
        //                tw.WriteLine($"     - Место стоянки {parking.Number}");
        //            }
        //            tw.WriteLine($" Требуемое количество СНО на блок по типам: стремянки {pb.GetGseCountByType(GseType.ladder)}, " +
        //                         $"упорные колодки {pb.GetGseCountByType(GseType.block)}, " +
        //                         $"конуса безопасности {pb.GetGseCountByType(GseType.markerCone)}, " +
        //                         $"буксировочные водила {pb.GetGseCountByType(GseType.towBar)}.");
        //        }

        //        tw.WriteLine($"Итоговое количество блоков: {oM.ParkingBlocks.Count()}");
        //        tw.WriteLine($"Общее потребное количество СНО по типам: стремянки {oM.GetGseCountByType(GseType.ladder)}, " +
        //                     $"упорные колодки {oM.GetGseCountByType(GseType.block)}, " +
        //                     $"конуса безопасности {oM.GetGseCountByType(GseType.markerCone)}, " +
        //                     $"буксировочные водила {oM.GetGseCountByType(GseType.towBar)}.");
        //    }
        //}

        #endregion


        /// <summary>
        /// Возвращает номера МС в блоке через запятую
        /// </summary>
        /// <returns></returns>
        internal static string GetParkingsByComma(ParkingBlock block)
        {
            var str = "";
            foreach (var parking in block.Parkings)
            {
                str += parking.Number + ",";
            }
            return str.TrimEnd(',');
        }
    }
}
