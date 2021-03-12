using OfficeOpenXml;
using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;

namespace DegreePrjWinForm.Managers
{
    /// <summary>
    /// Сервис для работы с MS Excel
    /// </summary>
    public static class ExcelService
    {

        public static void LoadData(string path, ExistingObjectManager objMgr)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var fi = new FileInfo(path); 
            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Schedule"];
                objMgr.ScheduleRows = worksheet.Tables.First().ConvertTableToObjects<ScheduleRowObject>().ToList();
                package.Save();

            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["PlaneParkings"];
                objMgr.ParkingObjects = worksheet.Tables.First().ConvertTablePPToObjects<PlaneParkingObject>().ToList();
                package.Save();
            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Planes"];
                objMgr.AircraftObjects = worksheet.Tables.First().ConvertTablePToObjects<AircraftObject>().ToList();
                package.Save();
            }
        }

    }
}
