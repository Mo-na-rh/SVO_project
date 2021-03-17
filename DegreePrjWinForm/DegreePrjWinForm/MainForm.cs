using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Extensions;
using DegreePrjWinForm.Managers;
using NLog;
using OfficeOpenXml;

namespace DegreePrjWinForm
{
    public partial class MainForm : Form
    {
        private ExistingObjectManager _objectManager;

        /// <summary>
        /// Логгер.
        /// </summary>
        internal readonly ILogger _logger;

        public MainForm()
        {
            InitializeComponent();

            _objectManager = new ExistingObjectManager();
            // initialize OpenFileDialog
            openFileDialog.FileName = "Select a shedule Excel file";
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Open Excel file";

            _logger = LogManager.GetCurrentClassLogger();
        }

        private void buttonSelectShedule_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxWorkPath.Text = openFileDialog.FileName;
            }
        }

        private void ComputeButton_Click(object sender, EventArgs e)
        {
            var pathResFile = textBoxResFilePath.Text; 
            var workFilePath = textBoxWorkPath.Text;

            ExcelService.LoadData(workFilePath,_objectManager);

            var pathToFile = @"D:\chetv_va\Диплом 2021\Данные для работы\Xml\";
            FillCoordinates(pathToFile);

            // Заполняются блоки по 3 парковочных места в одном
            FillParkingBlocks(_objectManager);

            ProcessingService.LinkRowObjectsToParkings(_objectManager);
            ProcessingService.CheckParkingBlocks(_objectManager);

            ReportService.WriteTestReport(pathResFile, _objectManager);

            MessageBox.Show("Report succesfully written!");
        }

        private void FillCoordinates(string pathToFile)
        {
            
                foreach (var o in _objectManager.ParkingObjects)
                {
                    var path = pathToFile + o.Number + ".xml";
                    try
                    {
                        XDocument xdoc = XDocument.Load(path);
                        XElement geozoneType = xdoc.Element("geozoneType");

                        XElement geometry = geozoneType.Elements("geometry").FirstOrDefault();
                        foreach (XElement phoneElement in geometry.Elements("point"))
                        {
                            XAttribute nameX = phoneElement.Attribute("x");
                            XAttribute nameY = phoneElement.Attribute("y");
                            if (nameX != null && nameY != null)
                            {
                                var coordObj = new CoordinateObject();
                                var englishCulture = CultureInfo.GetCultureInfo("en-US");
                                coordObj.X = double.Parse(nameX.Value, englishCulture);
                                coordObj.Y = double.Parse(nameY.Value, englishCulture);
                                o.Coordinates.Add(coordObj);
                            }
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.Trace("Файл не найден!" + ex.Message);
                    }
                }
        }



        private void FillParkingBlocks(ExistingObjectManager objMgr)
        {
            var i = 1;
            var block = new AircraftParkingsBlock();
            block.Id = i;
            block.AircraftParkings = new List<AircraftParkingObject>();
            objMgr.ParkingBlocks.Add(block);
            foreach (var parkingObject in objMgr.ParkingObjects)
            {
                if (i % 3 != 0)
                {
                    block.AircraftParkings.Add(parkingObject);
                }
                else
                {
                    block = new AircraftParkingsBlock { AircraftParkings = new List<AircraftParkingObject>(), Id = i };
                    block.AircraftParkings.Add(parkingObject);
                    objMgr.ParkingBlocks.Add(block);
                }

                i++;
            }
        }

        #region Form Events

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
