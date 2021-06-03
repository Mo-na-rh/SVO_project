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
            GseTypes.Add(GseType.towBar);
            //GseTypes.Add(GseType.stepway);
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

        #region computations

        /// <summary>
        /// Расчёт потребности в СНО на период 
        /// </summary>
        public void PredictGseDemandForAllFlightsInBlock()
        {
            // Цикл по всем МС внутри блока
            foreach (var parking in Parkings)
            {
                // Перебор всех рейсов по расписанию для МС
                foreach (var row in parking.LinkedScheduleRows)
                {
                    // Выявление количества комплектов по всем типам СНО
                    foreach (var gseType in GseTypes)
                    {
                        CheckAndAddIfNeedNewGseKitByGseType(row, gseType);
                    }
                }
            }
        }

        /// <summary>
        /// Необходим ли новый комплект СНО либо имеющихся достаточно, если необходим добавляем
        /// </summary>
        /// <param name="row"></param>
        /// <param name="gseType"></param>
        private void CheckAndAddIfNeedNewGseKitByGseType(ScheduleRow row, GseType gseType)
        {
            // получаем начальное и конечное время использования СНО
            var startDate = GetStartDateByGseType(row, gseType);
            var endDate = GetEndDateByGseType(row, gseType);

            if (IsNeedAddNewKit(startDate, endDate, gseType))
            {
                var gseKit = new GseKit(gseType);
                gseKit.CountGse = 4;
                gseKit.AddInUse(startDate, endDate);
                GseKits.Add(gseKit);
            }
        }

        /// <summary>
        /// Поиск свободного комплекта СНО, если найден загружаем возвращаем "нет" иначе "да"
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

        #endregion

        #region utility methods

        /// <summary>
        /// Получение времени начала использования СНО
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

            if (gseType == GseType.block)
            {
                var opStart = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Установка колодок");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opStart?.StartTime);
            }

            if (gseType == GseType.towBar)
            {
                if (row.Type == "D") //вылет
                {
                    return row.EndTGO;
                }

                return row.StartTGO - UtilityService.GetTimeSpanFromMinutes("00:10");
            }

            if (gseType == GseType.ladder)
            {
                var opStart = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Открытие грузовых люков");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opStart?.StartTime);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение времени окончания использования СНО
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

            if (gseType == GseType.block)
            {
                var opEnd = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Уборка колодок");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opEnd?.EndTime);
            }

            if (gseType == GseType.towBar)
            {
                if (row.Type == "D") //вылет
                {
                    return row.EndTGO + UtilityService.GetTimeSpanFromMinutes("00:10");
                }
                return row.StartTGO;
            }

            if (gseType == GseType.ladder)
            {
                var opEnd = row.LinkedTGO.Operations.FirstOrDefault(t => t.Name == "Закрытие грузовых люков");
                return row.StartTGO + UtilityService.GetTimeSpanFromMinutes(opEnd?.EndTime);
            }

            throw new NotImplementedException();
        }

        #endregion

    }
}
