using System;
using DegreePrjWinForm.Enums;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Строка расписания
    /// </summary>
    public class ScheduleRow
    {
        /// <summary>
        /// Дата рейса.
        /// </summary>
        public string FlightDate { get; set; }

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

        /// <summary>
        /// ТГО для воздушного судна по расписанию.
        /// </summary>
        public TGO LinkedTGO { get; set; }

        public string GetAirCompanyCode()
        {
            return CodeAirCompany;
        }

        public string GetAircraftBodyType()
        {
            return TypePlane;
        }

        public TgoType GetTgoType()
        {
            if (Type.Contains("D"))
                return TgoType.departure;

            if (Type.Contains("A"))
                return TgoType.arrival;

            return TgoType.reverse;
        }
    }
}
