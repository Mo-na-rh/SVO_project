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
        public List<AircraftParkingObject> ParkingObjects;
        public List<AircraftObject> AircraftObjects;
        public List<AircraftParkingsBlock> ParkingBlocks;

        public ExistingObjectManager()
        {
            ScheduleRows = new List<ScheduleRowObject>();
            ParkingObjects = new List<AircraftParkingObject>();
            AircraftObjects = new List<AircraftObject>();
            ParkingBlocks = new List<AircraftParkingsBlock>();
        }
    }
}
