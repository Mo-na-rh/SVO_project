using System;
using DegreePrjWinForm.Classes;
using System.Collections.Generic;

namespace DegreePrjWinForm.Managers
{
    /// <summary>
    /// Менеджер существующих объектов
    /// </summary>
    public class ObjectManager
    {
        public DateTime FromDate;

        public DateTime ToDate;

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

        public List<TGO> TgoObjects;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ObjectManager()
        {
            ScheduleRows = new List<ScheduleRow>();
            Parkings = new List<Parking>();
            Aircrafts = new List<Aircraft>();
            ParkingBlocks = new List<ParkingBlock>();
            TgoObjects = new List<TGO>();
        }

        public int GetGseCountByType(GseType type)
        {
            //TODO now plug will add logic
            var count = 0;
            foreach (var pb in ParkingBlocks)
            {
                foreach (var p in pb.Parkings)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
