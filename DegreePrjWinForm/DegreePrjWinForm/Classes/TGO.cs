﻿using System.Collections.Generic;

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
        public string TotalTime { get; set; }

        /// <summary>
        /// Код авиакомпании
        /// </summary>
        public string AirCompanyCode { get; set; }

        /// <summary>
        /// Наименование ТГО
        /// </summary>
        public TgoType Type { get; set; }

        /// <summary>
        /// Список СНО используемых в данном ТГО
        /// </summary>
        public List<GSE> GseList  = new List<GSE>();

        /// <summary>
        /// Список операций производимых в данном ТГО
        /// </summary>
        public List<Operation> Operations = new List<Operation>();
    }
}