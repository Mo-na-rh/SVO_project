using System;
using System.Collections.Generic;

namespace DegreePrjWinForm.Classes
{
    public class Operation
    {
        /// <summary>
        /// Наименование операции
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Общее время выполнения операции
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Список СНО используемых в данном ТГО
        /// </summary>
        public List<GSE> GseList = new List<GSE>();
    }
}