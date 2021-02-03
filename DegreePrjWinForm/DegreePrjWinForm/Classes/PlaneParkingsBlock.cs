using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Classes
{
    public class PlaneParkingsBlock
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Места стоянки входящие в блок.
        /// </summary>
        public List<PlaneParkingObject> PlaneParkings { get; set; }

    }
}
