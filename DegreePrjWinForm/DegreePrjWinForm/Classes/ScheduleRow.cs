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

        /// <summary>
        /// Есть ли пересечения с другими ТГО внутри блока (false по умолчанию)
        /// </summary>
        public bool IsCrossed { get; set; } = false;

        /// <summary>
        /// Начало техобслуживания по графику
        /// </summary>
        public DateTime StartTGO { get; set; }

        /// <summary>
        /// Окончание тех обслуживания по графику
        /// </summary>
        public DateTime EndTGO { get; set; }

        /// <summary>
        /// Получение типа ТГО
        /// </summary>
        /// <returns></returns>
        public TgoType GetTgoType()
        {
            if (Type.Contains("D"))
                return TgoType.departure;

            if (Type.Contains("A"))
                return TgoType.arrival;

            return TgoType.reverse;
        }

        /// <summary>
        /// Получение времени начала ТГО
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartDate()
        {
            var date = Convert.ToDateTime(FlightDate);
            var time = Convert.ToDateTime(FlightScheduleTime);
            StartTGO = date.Add(time.TimeOfDay);

            return StartTGO;
        }
    }
}
