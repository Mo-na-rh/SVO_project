using DegreePrjWinForm.Managers;
using System;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис по работе с объектами моделирования
    /// </summary>
    public static class ProcessingService
    {
        /// <summary>
        /// Привязка строк расписания к местам стоянок
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
        /// Проверка если хотябы одно МС не пустует то Блок считать заполненным
        /// </summary>
        /// <param name="objectMgr"></param>
        public static void CheckParkingBlocks(ObjectManager objectMgr)
        {
            foreach (var aircraftParkingsBlock in objectMgr.ParkingBlocks)
            {
                foreach (var parkingObject in aircraftParkingsBlock.AircraftParkings)
                {
                    if (parkingObject.LinkedScheduleRows.Count <= 0) continue;
                    aircraftParkingsBlock.IsFilled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Привязка ТГО к строкам расписания на каждую строку один ТГО
        /// </summary>
        /// <param name="objectManager"></param>
        internal static void LinkTgoToScheduleRows(ObjectManager objectManager)
        {
            throw new NotImplementedException();
        }
    }
}
