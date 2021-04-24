using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Enums;
using DegreePrjWinForm.Managers;
using System;
using System.Collections.Generic;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис по работе с объектами моделирования
    /// </summary>
    public static class ProcessingService
    {
        /// <summary>
        /// Обработка блоков, расчёт количеств СНО по блокам
        /// </summary>
        /// <param name="objectManager"></param>
        public static void HandleBlocks(ObjectManager objectManager)
        {
            SetStartAndFinishTGO(objectManager);

            foreach (var block in objectManager.ParkingBlocks)
            {
                HandleBlock(block);
            }
        }

        private static void HandleBlock(ParkingBlock block)
        {
            CheckIntersections(block);

            foreach (var parking in block.Parkings)
            {

            }
        }

        private static void CheckIntersections(ParkingBlock block)
        {
            foreach(var parking in block.Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach(var park in block.Parkings)
                    {
                        if (park == parking) continue;

                        foreach (var row2 in park.LinkedScheduleRows)
                        {
                            if( IsDatesCrossed(row.StartTGO, row.EndTGO, row2.StartTGO, row2.EndTGO) )
                            {
                                row2.IsCrossed = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка пересекаются ли диапазоны дат 
        /// </summary>
        /// <param name="sd1"></param>
        /// <param name="ed1"></param>
        /// <param name="sd2"></param>
        /// <param name="ed2"></param>
        /// <returns></returns>
        public static bool IsDatesCrossed(DateTime sd1, DateTime ed1, DateTime sd2, DateTime ed2)
        {
            return !((sd2>ed1)||(ed2<sd1));
        }

        /// <summary>
        /// Установка начала и конца ТГО по строкам расписания
        /// </summary>
        /// <param name="objectManager"></param>
        private static void SetStartAndFinishTGO(ObjectManager objectManager)
        {
            foreach (var block in objectManager.ParkingBlocks)
            {
                foreach (var parking in block.Parkings)
                {
                    foreach (var row in parking.LinkedScheduleRows)
                    {
                        row.StartTGO = row.GetStartDate();

                        if (row.LinkedTGO.Type == TgoType.departure)
                        {
                            // Если отправление отнимаем
                            row.EndTGO = row.StartTGO;
                            row.StartTGO = row.StartTGO.Subtract(row.LinkedTGO.GetTotalTime().TimeOfDay);
                        }
                        else
                        {
                            // если прибытие прибавляем
                            row.EndTGO = row.StartTGO.Add(row.LinkedTGO.GetTotalTime().TimeOfDay);
                        }
                    }
                }
            }
        }

    }
}
