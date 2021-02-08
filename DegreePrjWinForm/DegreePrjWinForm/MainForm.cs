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
            var aircraftObjects = new List<PlaneObject>();
            var planeParkingsBlocksObjects = new List<PlaneParkingsBlock>();

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

            // Заполняются блоки по 3 парковочных места в одном
            FillParkingBlocks(planeParkingsBlocksObjects, planeParkingObjects);

            CheckParkingBlocks(planeParkingsBlocksObjects, scheduleRowObjects);

            var pb = planeParkingsBlocksObjects.Where(t => !t.IsFilled);

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Planes"];
                aircraftObjects = worksheet.Tables.First().ConvertTablePToObjects<PlaneObject>().ToList();
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
                        $"num: {i} / FlightDate: {DateTime.Parse(row.FlightDate).ToShortDateString()} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToShortTimeString()} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
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
                foreach (var row in aircraftObjects)
                {
                    tw.WriteLine($"num: {row.Id} / IATA: {row.IATA} / ICAO: {row.ICAO} / Rus: {row.RUS} / Name: {row.Name} ");
                }

                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Empty parkings");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in pb)
                {
                    tw.WriteLine($"id block: {row.Id}");
                    foreach (var parking in row.PlaneParkings)
                    {
                        tw.WriteLine($" num: {parking.Id} / Number: {parking.Number} ");
                    }
                }
            }

            MessageBox.Show("Files succesfully written!");
        }

        private void CheckParkingBlocks(List<PlaneParkingsBlock> planeParkingsBlocksObjects, List<ScheduleRowObject> scheduleRowObjects)
        {
            foreach (var ppb in planeParkingsBlocksObjects)
            {
                foreach (var row in scheduleRowObjects)
                {
                    if (ppb.PlaneParkings.FirstOrDefault(t => t.Number == row.ParkingPlane) != null)
                    {
                        ppb.IsFilled = true;
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FillParkingBlocks(List<PlaneParkingsBlock> planeParkingsBlocksObjects, List<PlaneParkingObject> planeParkingObjects)
        {
            var i = 1;
            var block = new PlaneParkingsBlock();
            block.Id = i;
            block.PlaneParkings = new List<PlaneParkingObject>();
            planeParkingsBlocksObjects.Add(block);
            foreach (var parkingObject in planeParkingObjects)
            {
                if (i % 3 != 0)
                {
                    block.PlaneParkings.Add(parkingObject);
                }
                else
                {
                    block = new PlaneParkingsBlock();
                    block.PlaneParkings = new List<PlaneParkingObject>();
                    block.Id = i;
                    block.PlaneParkings.Add(parkingObject);
                    planeParkingsBlocksObjects.Add(block);
                }

                i++;
            }
        }
    }

    
}
