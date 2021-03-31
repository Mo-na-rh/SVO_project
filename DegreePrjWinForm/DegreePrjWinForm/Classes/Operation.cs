namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Объект операции
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Наименование операции
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип СНО
        /// </summary>
        public string GseType { get; set; }

        /// <summary>
        /// Время начала операции
        /// </summary>
        public string StartTime { get; set; }
        
        /// <summary>
        /// Время окончания операции
        /// </summary>
        public string EndTime { get; set; }
    }
}