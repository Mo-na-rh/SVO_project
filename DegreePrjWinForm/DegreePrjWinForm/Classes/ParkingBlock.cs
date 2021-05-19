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
        /// Есть ли рейсы по расписанию для стоянок внутри блока
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Места стоянки входящие в блок.
        /// </summary>
        public List<Parking> Parkings { get; set; }

        /// <summary>
        /// Количество СНО типа упорные колодки на блок
        /// </summary>
        public int BlockGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа стремянки на блок
        /// </summary>
        public int LadderGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа конуса безопасности на блок
        /// </summary>
        public int MarkerConeGseCount { get; set; }

        /// <summary>
        /// Количество СНО типа буксировочное водило на блок 
        /// </summary>
        public int TowBarGseCount { get; set; }

        /// <summary>
        /// Получение количества СНО на блок по типу
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetGseCountByType(GseType type)
        {
            var gseCount = 0;

            // Если ни одной нет операции с этим GSE то возвращаем 0 
            if (!IsExistSingleByType(type)) return 0;

            gseCount++;

            foreach (var parking in Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    if (row.IsUsedGseByType(type))
                    {
                        gseCount++;
                    }
                }
            }

            if (gseCount > Parkings.Count)
                gseCount = Parkings.Count;

            return gseCount;
        }

        /// <summary>
        /// Существует ли хоть одно использование СНО данного типа в  этом блоке
        /// </summary>
        /// <param name="gseType"></param>
        /// <returns></returns>
        private bool IsExistSingleByType(GseType gseType)
        {
            switch (gseType)
            {
                case GseType.block:
                    return IsExistBlock();
                case GseType.ladder:
                    return IsExistLadder();
                case GseType.towBar:
                    return IsExistTowBar();
                case GseType.markerCone:
                    return IsExistMarkerCone();
                default:
                    return false;
            }    
        }

        /// <summary>
        /// Существуют ли хоть одни колодки на блок
        /// </summary>
        /// <returns></returns>
        private bool IsExistBlock()
        {
            foreach (var parking in Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach (var operation in row.LinkedTGO.Operations)
                    {
                        if (string.Equals(operation.Name, "Установка колодок")) return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Существует ли хоть одна стремянка на блок
        /// </summary>
        /// <returns></returns>
        private bool IsExistLadder()
        {
            foreach (var parking in Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach (var operation in row.LinkedTGO.Operations)
                    {
                        if (string.Equals(operation.Name, "Открытие грузовых люков")) return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Существует ли хоть одно буксировочное водило на блок
        /// </summary>
        /// <returns></returns>
        private bool IsExistTowBar()
        {
            foreach (var parking in Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach (var operation in row.LinkedTGO.Operations)
                    {
                        if (string.Equals(operation.Name, "Прибытие ВС")) return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// существует ли хоть один комплект конусов безопасности на блок
        /// </summary>
        /// <returns></returns>
        private bool IsExistMarkerCone()
        {
            foreach (var parking in Parkings)
            {
                foreach (var row in parking.LinkedScheduleRows)
                {
                    foreach (var operation in row.LinkedTGO.Operations)
                    {
                        if (string.Equals(operation.Name, "Установка конусов безопасности")) return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Возвращает номера МС в блоке через запятую
        /// </summary>
        /// <returns></returns>
        internal string GetParkingsByComma()
        {
            var str = "";
            foreach (var parking in Parkings)
            {
                str += parking.Number + ",";
            }
            return str.TrimEnd(',');
        }
    }
}
