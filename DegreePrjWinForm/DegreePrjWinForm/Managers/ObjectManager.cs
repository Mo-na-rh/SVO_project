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
        /// <summary>
        /// Начало диапазона моделирования
        /// </summary>
        public DateTime FromDate;

        /// <summary>
        /// Окончание диапазона моделирования
        /// </summary>
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

        /// <summary>
        /// Все ТГО
        /// </summary>
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
