using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    public class AircraftParkingObject
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
        public List<CoordinateObject> Coordinates = new List<CoordinateObject>();

        /// <summary>
        /// G
        /// </summary>
        public List<ScheduleRowObject> LinkedScheduleRows = new List<ScheduleRowObject>();

        public double MiddleX()
        {
            double sum = 0;
            foreach (var c in Coordinates)
            {
                sum += c.X;
            }

            return sum / Coordinates.Count;
        }

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

    public class CoordinateObject
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
