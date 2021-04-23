using System;
using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Блок стоянок
    /// </summary>
    public class ParkingBlock
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заполнен ли блок.
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Места стоянки входящие в блок.
        /// </summary>
        public List<Parking> Parkings { get; set; }

        /// <summary>
        /// Получение количества СНО на блок по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetGseCountByType(GseType type)
        {
            //TODO now plug will add logic
            return Parkings.Count;
        }

    }
}
