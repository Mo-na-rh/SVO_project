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
        private List<ScheduleRowObject> _scheduleRowObjects;
        private List<PlaneParkingObject> _planeParkingObjects;
        private List<AircraftObject> _aircraftObjects;

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
                textBoxWorkPath.Text = openFileDialog.FileName;
            }
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            var pathResFile = textBoxResFilePath.Text; //@"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\Results.txt";

            _scheduleRowObjects = new List<ScheduleRowObject>();
            _planeParkingObjects = new List<PlaneParkingObject>();
            _aircraftObjects = new List<AircraftObject>();
            var planeParkingsBlocksObjects = new List<PlaneParkingsBlock>();

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var fi = new FileInfo(textBoxWorkPath.Text); //@"D:\chetv_va\ВУЗ\Диплом 2021\Данные для работы\work.xlsx"
            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Schedule"];
                _scheduleRowObjects = worksheet.Tables.First().ConvertTableToObjects<ScheduleRowObject>().ToList();
                package.Save();
              
            }

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["PlaneParkings"];
                _planeParkingObjects = worksheet.Tables.First().ConvertTablePPToObjects<PlaneParkingObject>().ToList();
                package.Save();
            }

            // Заполняются блоки по 3 парковочных места в одном
            FillParkingBlocks(planeParkingsBlocksObjects, _planeParkingObjects);

            CheckParkingBlocks(planeParkingsBlocksObjects, _scheduleRowObjects);

            var parkingBlocks = planeParkingsBlocksObjects.Where(t => !t.IsFilled);

            using (var package = new ExcelPackage(fi))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets["Planes"];
                _aircraftObjects = worksheet.Tables.First().ConvertTablePToObjects<AircraftObject>().ToList();
                package.Save();
            }

            WriteToTxtFile(pathResFile, parkingBlocks);

            MessageBox.Show("Files succesfully written!");
        }

        private void WriteToTxtFile(string pathResFile, IEnumerable<PlaneParkingsBlock> parkingBlocks)
        {
            var fi1 = new FileInfo(pathResFile);
            using (TextWriter tw = new StreamWriter(fi1.Open(FileMode.Truncate)))
            {
                tw.WriteLine(" Flights");
                tw.WriteLine(" ============================================================================================");
                var i = 0;
                foreach (var row in _scheduleRowObjects)
                {
                    tw.WriteLine(
                        $"num: {i} / FlightDate: {DateTime.Parse(row.FlightDate).ToShortDateString()} / FlightScheduleTime: {DateTime.Parse(row.FlightScheduleTime).ToShortTimeString()} / CodeAirCompany: {row.CodeAirCompany} / FlightNumber: {row.FlightNumber} / Type: {row.Type} / TypePlane: {row.TypePlane} / ParkingPlane: {row.ParkingPlane} / ParkingSector: {row.ParkingSector} / AirCompanyName: {row.AirCompanyName}");
                    i++;
                }
                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Plane parkings");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in _planeParkingObjects)
                {
                    tw.WriteLine($"num: {row.Id} / Number: {row.Number} ");
                }

                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Planes");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in _aircraftObjects)
                {
                    tw.WriteLine($"num: {row.Id} / IATA: {row.IATA} / ICAO: {row.ICAO} / Rus: {row.RUS} / Name: {row.Name} ");
                }

                tw.WriteLine(" ============================================================================================");
                tw.WriteLine(" Empty parkings");
                tw.WriteLine(" ============================================================================================");
                foreach (var row in parkingBlocks)
                {
                    tw.WriteLine($"id block: {row.Id}");
                    foreach (var parking in row.PlaneParkings)
                    {
                        tw.WriteLine($" num: {parking.Id} / Number: {parking.Number} ");
                    }
                }
            }
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
