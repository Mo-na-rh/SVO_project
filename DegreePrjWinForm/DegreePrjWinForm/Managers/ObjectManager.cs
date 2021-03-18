using DegreePrjWinForm.Classes;
using System.Collections.Generic;

namespace DegreePrjWinForm.Managers
{
    /// <summary>
    /// Менеджер существующих объектов
    /// </summary>
    public class ObjectManager
    {
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
        /// Конструктор класса
        /// </summary>
        public ObjectManager()
        {
            ScheduleRows = new List<ScheduleRow>();
            Parkings = new List<Parking>();
            Aircrafts = new List<Aircraft>();
            ParkingBlocks = new List<ParkingBlock>();
        }
    }
}
