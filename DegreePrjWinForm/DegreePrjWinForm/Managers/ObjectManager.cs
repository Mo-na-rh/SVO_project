using System;
using DegreePrjWinForm.Classes;
using System.Collections.Generic;
using System.Linq;

namespace DegreePrjWinForm.Managers
{
    /// <summary>
    /// Менеджер существующих объектов
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// Начало диапазона моделирования
        /// </summary>
        public DateTime FromDate;

        /// <summary>
        /// Окончание диапазона моделирования
        /// </summary>
        public DateTime ToDate;

        /// <summary>
        /// Количество стоянок в блоке
        /// </summary>
        public int CountParkingInBlock { get; set; }

        /// <summary>
        /// Строки расписания
        /// </summary>
        public List<ScheduleRow> ScheduleRows;

        /// <summary>
        /// Места стоянок
        /// </summary>
        public List<Parking> Parkings;

        /// <summary>
        /// Воздушные суда
        /// </summary>
        public List<Aircraft> Aircrafts;

        /// <summary>
        /// Места стоянок
        /// </summary>
        public List<ParkingBlock> ParkingBlocks;

        /// <summary>
        /// Все ТГО
        /// </summary>
        public List<TGO> TgoObjects;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ObjectManager(DateTime from, DateTime to, int countParking)
        {
            FromDate = from;
            ToDate = to;
            CountParkingInBlock = countParking;

            ScheduleRows = new List<ScheduleRow>();
            Parkings = new List<Parking>();
            Aircrafts = new List<Aircraft>();
            ParkingBlocks = new List<ParkingBlock>();
            TgoObjects = new List<TGO>();
        }

        internal string GetReportFileName()
        {
            // input
            //var startDateTime = new DateTime(2015, 5, 01);
            //var endDateTime = new DateTime(2015, 5, 3);
            //var countPB = 3;

            // processing

            var year = DateTime.Now.ToString("yy");
            var month = DateTime.Now.ToString("MM");
            var day = DateTime.Now.ToString("dd");

            var sDay = FromDate.ToString("dd");
            var sMonth = FromDate.ToString("MM");

            var endDay = ToDate.ToString("dd");
            var endMonth = ToDate.ToString("MM");

            var fileName = year + month + day + " ШХ - Прогноз потребности в СНО на " +
                sDay + sMonth + " - " + endDay + endMonth + " - " + CountParkingInBlock.ToString() + "МС";

            return fileName;
        }

        internal List<ScheduleRow> GetScheduleRows()
        {
            return ScheduleRows.Where(t => (FromDate <= Convert.ToDateTime(t.FlightDate))&& (ToDate >= Convert.ToDateTime(t.FlightDate))).ToList();
        }

        /// <summary>
        /// Расчёт суммы всего СНО в зависимости от типа
        /// </summary>
        /// <param name="type">Тип СНО</param>
        /// <returns>Количество СНО заданного типа</returns>
        public int GetGseCountByType(GseType type)
        {
            var count = 0;
            foreach (var block in ParkingBlocks)
            {
                count += block.GetGseCountByType(type);
            }

            return count;
        }
    }
}
