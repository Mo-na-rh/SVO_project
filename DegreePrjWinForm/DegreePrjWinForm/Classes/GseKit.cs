namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Комплект СНО
    /// </summary>
    public class GseKit
    {
        /// <summary>
        /// Занят ли сейчас комплект
        /// </summary>
        public bool IsBusy { get; set; } = false;

        /// <summary>
        /// Если сейчас занят на рейсе то на каком
        /// </summary>
        public ScheduleRow FlightOnProcessing { get; set; }

        /// <summary>
        /// Количество СНО входящее в комплект
        /// </summary>
        public int CountGse { get; set; }
        
        /// <summary>
        /// Тип СНО
        /// </summary>
        public GseType GseType { get; set; }

        /// <summary>
        /// Предназначено ли для широко-фюзеляжных ВС
        /// </summary>
        public bool IsWideBody { get; set; }
    }
}