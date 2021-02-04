using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Classes
{
    public class ScheduleRowObject
    {
        /// <summary>
        /// Дата рейса.
        /// </summary>
        public string FlightDate
        {
            get; set;
        }

        /// <summary>
        /// Время по расписанию.
        /// </summary>
        public string FlightScheduleTime { get; set; }

        /// <summary>
        /// Код авиакомпании.
        /// </summary>
        public string CodeAirCompany { get; set; }

        /// <summary>
        /// Номер рейса.
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// Флаг прилет/вылет.
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// Тип ВС IATA.
        /// </summary>
        public string TypePlane { get; set; }

        /// <summary>
        /// Стоянка ВС.
        /// </summary>
        public string ParkingPlane { get; set; }

        /// <summary>
        /// Стоянка сектор.
        /// </summary>
        public string ParkingSector { get; set; }


        /// <summary>
        /// Название авиакомпании.
        /// </summary>
        public string AirCompanyName { get; set; }
    }
}
