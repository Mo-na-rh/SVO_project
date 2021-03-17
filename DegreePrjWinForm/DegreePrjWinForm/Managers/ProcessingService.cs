using DegreePrjWinForm.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreePrjWinForm.Managers
{
    public static class ProcessingService
    {
        public static void LinkRowObjectsToParkings(ExistingObjectManager objectMgr)
        {
            foreach (var parkingObject in objectMgr.ParkingObjects)
            {
                foreach (var row in objectMgr.ScheduleRows)
                {
                    if (parkingObject.Number == row.ParkingPlane)
                    {
                        parkingObject.LinkedScheduleRows.Add(row);
                    }
                }
            }

        }

        public static void CheckParkingBlocks(ExistingObjectManager objectMgr)
        {
            foreach (var aircraftParkingsBlock in objectMgr.ParkingBlocks)
            {
                foreach (var parkingObject in aircraftParkingsBlock.AircraftParkings)
                {
                    if (parkingObject.LinkedScheduleRows.Count <= 0) continue;
                    aircraftParkingsBlock.IsFilled = true;
                    break;
                }
            }
        }
    }
}
