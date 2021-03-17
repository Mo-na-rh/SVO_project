
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;

namespace DegreePrjWinForm.Managers
{
    public static class ReportService
    {
        public static void WriteTestReport(string pathToResFile, ExistingObjectManager objMgr)
        {
            var fi = new FileInfo(pathToResFile);
            using (TextWriter tw = new StreamWriter(fi.Open(FileMode.Truncate)))
            {
                WriteFlights(tw, objMgr.ScheduleRows);
                WritePlaneParkings(tw, objMgr.ParkingObjects);
                WriteAircrafts(tw, objMgr.AircraftObjects);
                WriteEmptyParkings(tw, objMgr.ParkingBlocks);
            }
        }

        private static void WriteFlights(TextWriter tw, List<ScheduleRowObject> _scheduleRowObjects)
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

        private static void WritePlaneParkings(TextWriter tw, List<AircraftParkingObject> _planeParkingObjects)
        {
            tw.WriteLine(" ============================================================================================");
            tw.WriteLine(" Plane parkings");
            tw.WriteLine(" ============================================================================================");
            foreach (var parking in _planeParkingObjects)
            {
                tw.WriteLine($"num: {parking.Id} / Number: {parking.Number} / MiddleX: {parking.MiddleX()} / MiddleY: {parking.MiddleY()}");
                foreach (var row in parking.LinkedScheduleRows)
                {
                    tw.WriteLine(
                        $"  FlightDate: {DateTime.Parse(row.FlightDate).ToShortDateString()} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToShortTimeString()} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                }
            }
        }

        private static void WriteAircrafts(TextWriter tw, List<AircraftObject> _aircraftObjects)
        {
            tw.WriteLine(" ============================================================================================");
            tw.WriteLine(" Aircrafts");
            tw.WriteLine(" ============================================================================================");
            foreach (var row in _aircraftObjects)
            {
                tw.WriteLine($"num: {row.Id} / IATA: {row.IATA} / ICAO: {row.ICAO} / Rus: {row.RUS} / Name: {row.Name} ");
            }
        }

        private static void WriteEmptyParkings(TextWriter tw, List<AircraftParkingsBlock> _parkingBlocks)
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

    }
}
