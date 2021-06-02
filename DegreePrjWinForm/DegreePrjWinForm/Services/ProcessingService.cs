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

        /// <summary>
        /// Основной метод здесь расчитывается количество СНО на 1 блок МС на заданный период времени
        /// </summary>
        /// <param name="block"></param>
        private static void HandleBlock(ParkingBlock block)
        {
            block.CheckAndInitializeIntersections();

            //block.BlockGseCount = block.GetGseCountByType(GseType.block);
            //block.LadderGseCount = block.GetGseCountByType(GseType.ladder);
            //block.MarkerConeGseCount = block.GetGseCountByType(GseType.markerCone);
            //block.TowBarGseCount = block.GetGseCountByType(GseType.towBar);
        }

        private static void InitializeIntersectionsByGseTypes(ScheduleRow row, ScheduleRow row2)
        {
            //CheckBlockIntersection(row, row2);
            //CheckLadderIntersection(row, row2);
            //CheckMarkerconeIntersection(row, row2);
            //CheckTowBarIntersection(row, row2);
        }

        private static void CheckBlockIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Установка колодок").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Уборка колодок").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Установка колодок").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Уборка колодок").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (UtilityService.IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsBlockUsed)
            {
                row2.IsBlockUsed = true;
            }
        }

        private static void CheckTowBarIntersection(ScheduleRow row, ScheduleRow row2)
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
                endDate1 = row.EndTGO + UtilityService.GetTimeSpanFromMinutes("00:10");
            }
            else
            {
                startDate1 = row.StartTGO - UtilityService.GetTimeSpanFromMinutes("00:10");
                endDate1 = row.StartTGO;
            }

            if (row2.Type == "D") //вылет
            {
                startDate2 = row2.EndTGO;
                endDate2 = row2.EndTGO + UtilityService.GetTimeSpanFromMinutes("00:10");
            }
            else
            {
                startDate2 = row2.StartTGO - UtilityService.GetTimeSpanFromMinutes("00:10");
                endDate2 = row2.StartTGO;
            }


            if (UtilityService.IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsTowBarUsed)
            {
                row2.IsTowBarUsed = true;
            }
        }

        /// <summary>
        /// Инициализация пересечения операций по типу СНО конус безопасности
        /// </summary>
        /// <param name="row"></param>
        /// <param name="row2"></param>
        private static void CheckMarkerconeIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Установка конусов безопасности").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Уборка конусов безопасности").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Установка конусов безопасности").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Уборка конусов безопасности").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (UtilityService.IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsLadderUsed)
            {
                row2.IsMarkerConeUsed = true;
            }
        }

        private static void CheckLadderIntersection(ScheduleRow row, ScheduleRow row2)
        {
            // получаем операции
            var operationStart1 = row.LinkedTGO.Operations.Where(t => t.Name == "Открытие грузовых люков").FirstOrDefault();
            var operationEnd1 = row.LinkedTGO.Operations.Where(t => t.Name == "Закрытие грузовых люков").FirstOrDefault();

            var operationStart2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Открытие грузовых люков").FirstOrDefault();
            var operationEnd2 = row2.LinkedTGO.Operations.Where(t => t.Name == "Закрытие грузовых люков").FirstOrDefault();

            // получаем время
            var startDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart1.StartTime);
            var endDate1 = row.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd1.EndTime);

            var startDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationStart2.StartTime);
            var endDate2 = row2.StartTGO + UtilityService.GetTimeSpanFromMinutes(operationEnd2.EndTime);

            if (UtilityService.IsDatesCrossed(startDate1, endDate1, startDate2, endDate2) && !row.IsLadderUsed)
            {
                row2.IsLadderUsed = true;
            }
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
