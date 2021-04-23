using System;
using DegreePrjWinForm.Enums;
using DegreePrjWinForm.Managers;

namespace DegreePrjWinForm.Services
{
    public static class LinkingService
    {
        /// <summary>
        /// Привязка строк расписания к МС
        /// </summary>
        /// <param name="objectManager"></param>
        public static void LinkScheduleRowsToParkings(ObjectManager objectManager)
        {
            foreach (var parkingObject in objectManager.Parkings)
            {
                foreach (var row in objectManager.ScheduleRows)
                {
                    if (parkingObject.Number == row.ParkingPlane)
                    {
                        parkingObject.LinkedScheduleRows.Add(row);
                    }
                }
            }
        }

        /// <summary>
        /// Привязка ТГО к строкам расписания на каждую строку один ТГО
        /// </summary>
        /// <param name="objectManager"></param>
        public static void LinkTgoToScheduleRows(ObjectManager objectManager)
        {
            foreach (var row in objectManager.ScheduleRows)
            {
                var aCCode = row.CodeAirCompany;
                var aircraftType = GetAircraftBodyType(row.TypePlane);
                var type = row.GetTgoType();

                foreach (var tgo in objectManager.TgoObjects)
                {
                    if((tgo.Type == type)  && (tgo.AircraftBodyType == aircraftType)) // TODO: && (string.Equals(tgo.ACCode, aCCode)) 
                    {
                        row.LinkedTGO = tgo;
                        break;
                    }
                }

            }
        }

        private static AircraftBodyType GetAircraftBodyType(string typePlane)
        {
            return AircraftBodyType.narrow;
        }
    }
}
