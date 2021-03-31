using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Managers;

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
                foreach (var parking in row.AircraftParkings)
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
        public static void WriteResultReport(string pathToResultReportFile, ObjectManager oM)
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
                    foreach (var parking in pb.AircraftParkings)
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



    }
}
