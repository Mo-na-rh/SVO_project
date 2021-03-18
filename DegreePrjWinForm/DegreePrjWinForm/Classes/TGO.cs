using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    /// <summary>
    /// Объект типа ТГО
    /// </summary>
    public class TGO
    {
        /// <summary>
        /// Общее время выполнения ТГО
        /// </summary>
        public string AllTime { get; set; }

        /// <summary>
        /// Наименование ТГО
        /// </summary>
        public TgoType Type { get; set; }

        /// <summary>
        /// Список СНО используемых в данном ТГО
        /// </summary>
        public List<GSE> GseList  = new List<GSE>();
    }
}