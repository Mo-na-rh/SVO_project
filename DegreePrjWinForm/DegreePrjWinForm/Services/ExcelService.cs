using System.IO;
using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;
using DegreePrjWinForm.Managers;
using OfficeOpenXml;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис для работы с MS Excel, загрузка расписания, типов самолётов и 
    /// </summary>
    public static class ExcelService
    {
        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <param name="path"></param>
        /// <param name="objMgr"></param>
        public static void LoadData(string path, ObjectManager objMgr)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var fi = new FileInfo(path); 
            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Schedule"];
                objMgr.ScheduleRows = worksheet.Tables.First().ConvertTableToObjects<ScheduleRow>().ToList();
                package.Save();

            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["AircraftParkings"];
                objMgr.Parkings = worksheet.Tables.First().ConvertTablePPToObjects<Parking>().ToList();
                package.Save();
            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Aircrafts"];
                objMgr.Aircrafts = worksheet.Tables.First().ConvertTablePToObjects<Aircraft>().ToList();
                package.Save();
            }
        }

    }
}
