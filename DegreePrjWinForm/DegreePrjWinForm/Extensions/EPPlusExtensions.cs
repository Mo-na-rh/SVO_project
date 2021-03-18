using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace DegreePrjWinForm.Extensions
{
    /// <summary>
    /// Расширения EPPlus.ExcelTable для получения объектов из Excel таблицы
    /// </summary>
    public static class EPPlusExtensions
    {
        /// <summary>
        /// Получение объектов расписание 
        /// </summary>
        /// <typeparam name="ScheduleRow"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<ScheduleRow> ConvertTableToObjects<ScheduleRow>(this ExcelTable table) where ScheduleRow : new()
        {
            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                    excelDate = excelDate - 2;
                else
                    excelDate = excelDate - 1;
                return dateOfReference.AddDays(excelDate);
            });

            //Get the properties of T
            var tprops = (new ScheduleRow())
                .GetType()
                .GetProperties()
                .ToList();

            //Get the cells based on the table address
            var start = table.Address.Start;
            var end = table.Address.End;
            var cells = new List<ExcelRangeBase>();

            //Have to use for loops insteadof worksheet.Cells to protect against empties
            for (var r = start.Row; r <= end.Row; r++)
                for (var c = start.Column; c <= end.Column; c++)
                    cells.Add(table.WorkSheet.Cells[r, c]);

            var groups = cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
                .ToList();

            //Everything after the header is data
            var rowvalues = groups.Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            var resList = new List<ScheduleRow>();
            foreach (var row in rowvalues)
            {
                var resRow = new ScheduleRow();

                foreach (var colName in colnames)
                {
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(resRow, row[colName.index].ToString());
                }

                resList.Add(resRow);
            }

            return resList;
        }

        /// <summary>
        /// Получение объектов мест стоянок воздушных судов
        /// </summary>
        /// <typeparam name="Parking"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<Parking> ConvertTablePPToObjects<Parking>(this ExcelTable table) where Parking : new()
        {

            //Get the properties of T
            var tprops = (new Parking())
                .GetType()
                .GetProperties()
                .ToList();

            //Get the cells based on the table address
            var start = table.Address.Start;
            var end = table.Address.End;
            var cells = new List<ExcelRangeBase>();

            //Have to use for loops insteadof worksheet.Cells to protect against empties
            for (var r = start.Row; r <= end.Row; r++)
                for (var c = start.Column; c <= end.Column; c++)
                    cells.Add(table.WorkSheet.Cells[r, c]);

            var groups = cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
                .ToList();

            //Everything after the header is data
            var rowvalues = groups.Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            var resList = new List<Parking>();
            foreach (var row in rowvalues)
            {
                var resRow = new Parking();
               
                foreach (var colName in colnames)
                {
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(resRow, row[colName.index].ToString());
                }

                resList.Add(resRow);
            }

            return resList;
        }

        /// <summary>
        /// Получение объектов воздушных судов
        /// </summary>
        /// <typeparam name="Aircraft"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<Aircraft> ConvertTablePToObjects<Aircraft>(this ExcelTable table) where Aircraft : new()
        {

            //Get the properties of T
            var tprops = (new Aircraft())
                .GetType()
                .GetProperties()
                .ToList();

            //Get the cells based on the table address
            var start = table.Address.Start;
            var end = table.Address.End;
            var cells = new List<ExcelRangeBase>();

            //Have to use for loops insteadof worksheet.Cells to protect against empties
            for (var r = start.Row; r <= end.Row; r++)
            for (var c = start.Column; c <= end.Column; c++)
                cells.Add(table.WorkSheet.Cells[r, c]);

            var groups = cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
                .ToList();

            //Everything after the header is data
            var rowvalues = groups.Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            var resList = new List<Aircraft>();
            foreach (var row in rowvalues)
            {
                var resRow = new Aircraft();

                foreach (var colName in colnames)
                {
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(resRow, row[colName.index].ToString());
                }

                resList.Add(resRow);
            }

            return resList;
        }
    }
}
