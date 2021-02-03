using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;
using OfficeOpenXml;

namespace DegreePrjWinForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // initialize OpenFileDialog
            openFileDialog.FileName = "Select a shedule Excel file";
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Open Excel file";
        }

        private void buttonSelectShedule_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialog.FileName;
                //
            }
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            var pathresfile = @"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\Results.txt";

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var fi = new FileInfo(@"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\work.xlsx");
            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();
                var scheduleRowObjects = worksheet.Tables.First().ConvertTableToObjects<ScheduleRowObject>().ToList();
                foreach (var data in scheduleRowObjects)
                {
                    Console.WriteLine(data.FlightDate + ":" + data.AirCompanyName + ":" + data.ParkingSector);
                }

                package.Save();

                var fi1 = new FileInfo(pathresfile);
                using (TextWriter tw = new StreamWriter(fi1.Open(FileMode.Truncate)))
                {
                    var i = 0;
                    foreach (var row in scheduleRowObjects)
                    {
                        tw.WriteLine(
                            $"num: {i} / FlightDate: {row.FlightDate} / FlightScheduleTime: {row.FlightScheduleTime} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                        i++;
                    }
                }

                package.Save();

                MessageBox.Show("File succesfully written!");
            }
        }


        //    using (TextWriter tw = new StreamWriter(pathfile))
        //    {
        //        foreach (var item in Data.List)
        //        {
        //            tw.WriteLine(string.Format("Item: {0} - Cost: {1}", item.Name, item.Cost.ToString()));
        //        }
        //    }
        //}
    }
}
