using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Служебные методы
    /// </summary>
    public static class UtilityService
    {
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
