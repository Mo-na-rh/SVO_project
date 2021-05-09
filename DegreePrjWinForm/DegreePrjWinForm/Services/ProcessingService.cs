using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Enums;
using DegreePrjWinForm.Managers;
using System;
using System.Linq;
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
            CheckAndInitializeIntersections(block);
            block.BlockGseCount = block.GetGseCountByType(GseType.block);
            block.LadderGseCount = block.GetGseCountByType(GseType.ladder);
            block.MarkerConeGseCount = block.GetGseCountByType(GseType.markerCone);
            block.TowBarGseCount = block.GetGseCountByType(GseType.towBar);
        }

        private static void CheckAndInitializeIntersections(ParkingBlock block)
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
                            if( IsDatesCrossed(row.StartTGO, row.EndTGO, row2.StartTGO, row2.EndTGO))
                            {
                                row2.IsCrossed = true;

                                InitializeIntersectionsByGseTypes(row, row2);
                                
                            }
                        }
                    }
                }
            }
        }

        private static void InitializeIntersectionsByGseTypes(ScheduleRow row, ScheduleRow row2)
        {
            InitializeBlockIntersection(row, row2);
            InitializeLadderIntersection(row, row2);
            InitializeMarkerconeIntersection(row, row2);
            InitializeTowBarIntersection(row, row2);
        }

        private static void InitializeTowBarIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // для прилета - 10 мин от прибытия ВС, для вылета + 10 мин к отправлению(прибытие,отправление операции в тго )
            // получаем операции
            var startDate1 = DateTime.Now;
            var endDate1 = DateTime.Now;
            var startDate2 = DateTime.Now;
            var endDate2 = DateTime.Now;


            if (row.Type == "D") //вылет
            {
                startDate1 = row.EndTGO;
                endDate1 = row.EndTGO + GetTimeSpanFromMinutes("00:10");
            }
            else
            {
                startDate1 = row.StartTGO - GetTimeSpanFromMinutes("00:10");
                endDate1 = row.StartTGO;
            }

            if (row2.Type == "D") //вылет
            {
                startDate2 = row2.EndTGO;
                endDate2 = row2.EndTGO + GetTimeSpanFromMinutes("00:10");
            }
            else
            {
                startDate2 = row2.StartTGO - GetTimeSpanFromMinutes("00:10");
                endDate2 = row2.StartTGO;
            }


            if (IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsTowBarUsed)
            {
                row2.IsTowBarUsed = true;
            }
        }

        /// <summary>
        /// string вида "mm:ss" на выход минуты
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static TimeSpan GetTimeSpanFromMinutes(string time)
        {
            // default format hh:mm
            var splittedTime = time.Split(':');
            return TimeSpan.FromMinutes(int.Parse(splittedTime[1]));
        }

        /// <summary>
        /// Инициализация пересечения операций по типу СНО конус безопасности
        /// </summary>
        /// <param name="row"></param>
        /// <param name="row2"></param>
        private static void InitializeMarkerconeIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Установка конусов безопасности").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Уборка конусов безопасности").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Установка конусов безопасности").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Уборка конусов безопасности").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsLadderUsed)
            {
                row2.IsMarkerConeUsed = true;
            }
        }

        private static void InitializeLadderIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Открытие грузовых люков").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Закрытие грузовых люков").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Открытие грузовых люков").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Закрытие грузовых люков").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsLadderUsed)
            {
                row2.IsLadderUsed = true;
            }
        }

        private static void InitializeBlockIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Установка колодок").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Уборка колодок").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Установка колодок").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Уборка колодок").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (IsDatesCrossed(startDate1, endDate1, startDate2, endDate2)&&!row.IsBlockUsed)
            {
                row2.IsBlockUsed = true;
            }
        }

        /// <summary>
        /// Проверка пересекаются ли два диапазона дат 
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
