namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Воздушное судно
    /// </summary>
    public class Aircraft
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Обозначение IATA.
        /// </summary>
        public string IATA { get; set; }

        /// <summary>
        /// Обозначение ICAO.
        /// </summary>
        public string ICAO { get; set; }

        /// <summary>
        /// Обозначение по-русски.
        /// </summary>
        public string RUS { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

    }
}