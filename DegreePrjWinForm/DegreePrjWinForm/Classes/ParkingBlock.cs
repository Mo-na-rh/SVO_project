using System;
using System.Collections.Generic;
using System.Linq;
using DegreePrjWinForm.Services;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Блок стоянок
    /// </summary>
    public class ParkingBlock
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Есть ли рейсы по расписанию для стоянок внутри блока
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Места стоянки входящие в блок.
        /// </summary>
        public List<Parking> Parkings { get; set; } = new List<Parking>();


        /// <summary>
        /// Комплекты СНО входящие в блок.
        /// </summary>
        public List<GseKit> GseKits { get; set; } = new List<GseKit>();

        /// <summary>
        /// Типы СНО входящие в блок.
        /// </summary>
        public List<GseType> GseTypes { get; set; } = new List<GseType>();


        /// <summary>
        /// Количество СНО типа упорные колодки на блок
        /// </summary>
        public int BlockGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа стремянки на блок
        /// </summary>
        public int LadderGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа конуса безопасности на блок
        /// </summary>
        public int MarkerConeGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа буксировочное водило на блок 
        /// </summary>
        public int TowBarGseCount { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParkingBlock()
        {
            //Заполнение списка типов СНО
            GseTypes.Add(GseType.markerCone);
            GseTypes.Add(GseType.ladder);
            GseTypes.Add(GseType.block);
            GseTypes.Add(GseType.stepway);
            GseTypes.Add(GseType.towBar);
        }

        /// <summary>
        /// Получение количества СНО на блок по типу
        /// </summary>
        /// <param name="gseType"></param>
        /// <returns></returns>
        public int GetGseCountByType(GseType gseType)
        {
            return GseKits.Where(t => t.GseType == gseType).Sum(gseKit => gseKit.CountGse);
        }


        #region Compution

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        public void CheckAndInitializeIntersections()
        {
            // цикл по всем МС внутри блока
            foreach (var parking in Parkings)
            {
                // цикл по всем рейсам внутри МС
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach (var gseType in GseTypes)
                    {
                        CheckIntersections(row, gseType);
                    }
                }
            }
        }

        private void CheckIntersections(ScheduleRow row, GseType gseType)
        {
            // получаем время использования СНО
            var startDate = GetStartDateByGseType(row, gseType);
            var endDate = GetEndDateByGseType(row, gseType);

            if (IsNeedAddNewKit(startDate, endDate, gseType))
            {
                var gseKit = new GseKit(gseType);
                gseKit.AddInUse(startDate, endDate);
                GseKits.Add(gseKit);
            }
        }

        /// <summary>
        /// Начало использования СНО
        /// </summary>
        /// <param name="row"></param>
        /// <param name="gseType"></param>
        /// <returns></returns>
        private DateTime GetStartDateByGseType(ScheduleRow row, GseType gseType)
        {
            if (gseType == GseType.markerCone)
            {
                var opStart = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Установка конусов безопасности");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opStart?.StartTime);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Окончание использования СНО
        /// </summary>
        /// <param name="row"></param>
        /// <param name="gseType"></param>
        /// <returns></returns>
        private DateTime GetEndDateByGseType(ScheduleRow row, GseType gseType)
        {
            if (gseType == GseType.markerCone)
            {
                var opEnd = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Уборка конусов безопасности");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opEnd?.EndTime);
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// Нужно ли добавлять новый комплект
        /// </summary>
        /// <param name="startDate1"></param>
        /// <param name="endDate1"></param>
        /// <param name="markerCone"></param>
        /// <returns></returns>
        private bool IsNeedAddNewKit(DateTime startDate1, DateTime endDate1, GseType gseType)
        {
            foreach (var gseKit in GseKits.Where(t=>t.GseType == gseType))
            {
                if (gseKit.IsBusedByDates(startDate1, endDate1))
                    continue;

                gseKit.AddInUse(startDate1, endDate1);
                return false;
            }

            return true;
        }

       


        /// <summary>
        /// Инициализация пересечения операций по типу СНО конус безопасности
        /// </summary>
        /// <param name="row"></param>
        /// <param name="row2"></param>
        private void CheckMarkerconeIntersection(ScheduleRow row, ScheduleRow row2)
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
                foreach (var gseKit in GseKits)
                {
                    //
                }

            }
        }

        #endregion


        ///// <summary>
        ///// Существуют ли хоть одни колодки на блок
        ///// </summary>
        ///// <returns></returns>
        //private bool IsExistBlock()
        //{
        //    foreach (var parking in Parkings)
        //    {
        //        foreach (var row in parking.LinkedScheduleRows)
        //        {
        //            foreach (var operation in row.LinkedTGO.Operations)
        //            {
        //                if (string.Equals(operation.Name, "Установка колодок")) return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Существует ли хоть одна стремянка на блок
        ///// </summary>
        ///// <returns></returns>
        //private bool IsExistLadder()
        //{
        //    foreach (var parking in Parkings)
        //    {
        //        foreach (var row in parking.LinkedScheduleRows)
        //        {
        //            foreach (var operation in row.LinkedTGO.Operations)
        //            {
        //                if (string.Equals(operation.Name, "Открытие грузовых люков")) return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Существует ли хоть одно буксировочное водило на блок
        ///// </summary>
        ///// <returns></returns>
        //private bool IsExistTowBar()
        //{
        //    foreach (var parking in Parkings)
        //    {
        //        foreach (var row in parking.LinkedScheduleRows)
        //        {
        //            foreach (var operation in row.LinkedTGO.Operations)
        //            {
        //                if (string.Equals(operation.Name, "Прибытие ВС")) return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// существует ли хоть один комплект конусов безопасности на блок
        ///// </summary>
        ///// <returns></returns>
        //private bool IsExistMarkerCone()
        //{
        //    foreach (var parking in Parkings)
        //    {
        //        foreach (var row in parking.LinkedScheduleRows)
        //        {
        //            foreach (var operation in row.LinkedTGO.Operations)
        //            {
        //                if (string.Equals(operation.Name, "Установка конусов безопасности")) return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
