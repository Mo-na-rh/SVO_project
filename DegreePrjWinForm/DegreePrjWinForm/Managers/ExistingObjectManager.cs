using DegreePrjWinForm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Managers
{
    public class ExistingObjectManager
    {
        public List<ScheduleRowObject> ScheduleRows;
        public List<PlaneParkingObject> ParkingObjects;
        public List<AircraftObject> AircraftObjects;
        public List<PlaneParkingsBlock> ParkingBlocks;

        public ExistingObjectManager()
        {
            ScheduleRows = new List<ScheduleRowObject>();
            ParkingObjects = new List<PlaneParkingObject>();
            AircraftObjects = new List<AircraftObject>();
            ParkingBlocks = new List<PlaneParkingsBlock>();
        }
    }
}
