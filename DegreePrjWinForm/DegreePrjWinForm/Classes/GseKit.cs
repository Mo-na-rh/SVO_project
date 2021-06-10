using DegreePrjWinForm.Services;
using System;
using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Комплект СНО
    /// </summary>
    public class GseKit
    {
        public GseKit(GseType type)
        {
            GseType = type;
        }

        /// <summary>
        /// Занят ли сейчас комплект
        /// </summary>
        public bool IsBusy { get; set; } = false;


        /// <summary>
        /// Количество СНО входящее в комплект
        /// </summary>
        public int CountGse { get; set; }
        
        /// <summary>
        /// Тип СНО
        /// </summary>
        public GseType GseType { get; set; }

        /// <summary>
        /// Предназначено ли для широко-фюзеляжных ВС
        /// </summary>
        public bool IsWideBody { get; set; }

        public List<GseBusyRange> BusyRanges { get; set; } = new List<GseBusyRange>();

        /// <summary>
        /// Преднозначено для проверки можно ли комплект СНО использовать в заданный период времени
        /// </summary>
        /// <param name="startDateTime">Начало использования</param>
        /// <param name="endDateTime">Окончание использования</param>
        /// <returns></returns>
        public bool IsBusedByDates(DateTime startDateTime, DateTime endDateTime)
        {
            foreach (var range in BusyRanges)
            {
                if (UtilityService.IsDatesCrossed(range.StartDate, range.EndDate, startDateTime, endDateTime))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Добавление времени занятости
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public void AddInUse(DateTime startDateTime, DateTime endDateTime)
        {
            var busyRange = new  GseBusyRange();
            busyRange.StartDate = startDateTime;
            busyRange.EndDate = endDateTime;
            busyRange.GseTypeInBusy = GseType;
            BusyRanges.Add(busyRange);
        }
    }

    /// <summary>
    /// Диапазон в котором задействовано СНО
    /// </summary>
    public class GseBusyRange
    {
        /// <summary>
        /// Начальное время
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Конечное время
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// тип сно который задействован
        /// </summary>
        public GseType GseTypeInBusy { get; set; }
    }
}