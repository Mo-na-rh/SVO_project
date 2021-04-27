using System;
using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Место стоянки воздушного судна
    /// </summary>
    public class Parking
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Обозначение стоянки.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Координаты стоянки.
        /// </summary>
        public List<Coordinate> Coordinates = new List<Coordinate>();

        /// <summary>
        /// Признак своим ходом туда добирается ВС или нет.
        /// </summary>
        public bool IsPropelled;

        /// <summary>
        /// Связанные Строки расписания
        /// </summary>
        public List<ScheduleRow> LinkedScheduleRows = new List<ScheduleRow>();

        /// <summary>
        /// Среднее по X коодрдинате
        /// </summary>
        /// <returns>число с плавающей точкой</returns>
        public double MiddleX()
        {
            double sum = 0;
            foreach (var c in Coordinates)
            {
                sum += c.X;
            }

            return sum / Coordinates.Count;
        }

        /// <summary>
        /// Среднее по Y коодрдинате
        /// </summary>
        /// <returns>число с плавающей точкой</returns>
        public double MiddleY()
        {
            double sum = 0;
            foreach (var c in Coordinates)
            {
                sum += c.Y;
            }

            return sum / Coordinates.Count;
        }

        public bool IsContact()
        {
            var utk = new string[] {
                "10", "11", "12", "13", "13A", "14", "15", "15A", "16", "17", "17A", "18", "19", "19A",
                "20", "21", "21A", "22", "23", "24", "25", "26", "27", "27A", "28", "29", "29A",
                "30", "31", "32", "32A", "33", "34", "34A", "35", "36", "37", "37A", "38", "39",
                "40", "41", "42", "42A", "42C", "43", "43A", "43C", "44",
                "44A", "44C", "45", "45A", "46", "47", "48", "48A", "49",
                "50", "51", "52", "53", "53A", "54", "55", "56", "57", "57A", "58", "59","60"
            };
            var stk = new string[] {
                "113", "114", "115", "116", "117", "118", "119",
                "120", "120A", "121", "122", "123", "124", "125", "126",
                "127", "128", "128A", "129", "130", "130A", "131"
            };

            return Array.Exists(utk, t => t == Number) || Array.Exists(stk, t => t == Number);
        }
    }
}
