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
            var pathResFile = @"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\Results.txt";

            var scheduleRowObjects = new List<ScheduleRowObject>();
            var planeParkingObjects = new List<PlaneParkingObject>();
            var planeObjects = new List<PlaneObject>();

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var fi = new FileInfo(@"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\work.xlsx");
            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Schedule"];
                scheduleRowObjects = worksheet.Tables.First().ConvertTableToObjects<ScheduleRowObject>().ToList();
                package.Save();
              
            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["PlaneParkings"];
                planeParkingObjects = worksheet.Tables.First().ConvertTablePPToObjects<PlaneParkingObject>().ToList();
                package.Save();
            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Planes"];
                planeObjects = worksheet.Tables.First().ConvertTablePToObjects<PlaneObject>().ToList();
                package.Save();
            }

            var fi1 = new FileInfo(pathResFile);
            using (TextWriter tw = new StreamWriter(fi1.Open(FileMode.Truncate)))
            {
                tw.WriteLine(" Flights");
                tw.WriteLine(" ============================================================================================");
                var i = 0;
                foreach (var row in scheduleRowObjects)
                {
                    tw.WriteLine(
                        $"num: {i} / FlightDate: {DateTime.Parse(row.FlightDate).Date} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToString("hh:mm")} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                    i++;
                }
                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Plane parkings");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in planeParkingObjects)
                {
                    tw.WriteLine($"num: {row.Id} / Number: {row.Number} ");
                }

                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Planes");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in planeObjects)
                {
                    tw.WriteLine($"num: {row.Id} / IATA: {row.IATA} / ICAO: {row.ICAO} / Rus: {row.RUS} / Name: {row.Name} ");
                }
            }

            MessageBox.Show("Files succesfully written!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
