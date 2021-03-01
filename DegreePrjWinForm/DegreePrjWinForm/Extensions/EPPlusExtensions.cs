using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegreePrjWinForm.Classes;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace DegreePrjWinForm.Extensions
{
    public static class EPPlusExtensions
    {
        public static IEnumerable<ScheduleRowObject> ConvertTableToObjects<ScheduleRowObject>(this ExcelTable table) where ScheduleRowObject : new()
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
            var tprops = (new ScheduleRowObject())
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

            var resList = new List<ScheduleRowObject>();
            foreach (var row in rowvalues)
            {
                var resRow = new ScheduleRowObject();

                foreach (var colName in colnames)
                {
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(resRow, row[colName.index].ToString());
                }

                resList.Add(resRow);
            }

            return resList;
        }

        public static IEnumerable<PlaneParkingObject> ConvertTablePPToObjects<PlaneParkingObject>(this ExcelTable table) where PlaneParkingObject : new()
        {

            //Get the properties of T
            var tprops = (new PlaneParkingObject())
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

            var resList = new List<PlaneParkingObject>();
            foreach (var row in rowvalues)
            {
                var resRow = new PlaneParkingObject();
               
                foreach (var colName in colnames)
                {
                    var prop = tprops.First(p => p.Name == colName.Name);
                    prop.SetValue(resRow, row[colName.index].ToString());
                }

                resList.Add(resRow);
            }

            return resList;
        }

        public static IEnumerable<PlaneObject> ConvertTablePToObjects<PlaneObject>(this ExcelTable table) where PlaneObject : new()
        {

            //Get the properties of T
            var tprops = (new PlaneObject())
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

            var resList = new List<PlaneObject>();
            foreach (var row in rowvalues)
            {
                var resRow = new PlaneObject();

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
