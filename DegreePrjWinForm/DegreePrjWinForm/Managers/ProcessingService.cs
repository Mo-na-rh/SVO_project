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
        public static void CheckParkingBlocks(ExistingObjectManager objectMgr)
        {
            foreach (var ppb in objectMgr.ParkingBlocks)
            {
                foreach (var row in objectMgr.ScheduleRows)
                {
                    if (ppb.PlaneParkings.FirstOrDefault(t => t.Number == row.ParkingPlane) != null)
                    {
                        ppb.IsFilled = true;
                        break;
                    }
                }
            }
        }
    }
}
