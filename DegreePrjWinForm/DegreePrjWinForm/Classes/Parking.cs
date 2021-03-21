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
    }
}
