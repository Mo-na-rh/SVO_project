using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Services
{
    public static class UtilityService
    {
        /// <summary>
        /// string вида "mm:ss" на выход минуты
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpanFromMinutes(string time)
        {
            // default format hh:mm
            var splittedTime = time.Split(':');
            return TimeSpan.FromMinutes(int.Parse(splittedTime[1]));
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
            return !((sd2 > ed1) || (ed2 < sd1));
        }
    }
}
