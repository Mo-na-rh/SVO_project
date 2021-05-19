using System;
using System.Windows.Forms;
using DegreePrjWinForm.Managers;
using DegreePrjWinForm.Services;
using NLog;

namespace DegreePrjWinForm
{
    /// <summary>
    /// Основная форма
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Класс для работы с объектами моделирования
        /// </summary>
        private ObjectManager _objectManager;

        /// <summary>
        /// Логгер.
        /// </summary>
        internal readonly ILogger _logger;

        /// <summary>
        /// Констуктор основной формы
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _logger = LogManager.GetCurrentClassLogger();

            openFileDialog.Filter = "Все файлы Excel (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Выбрать";
        }

        private void ComputeButton_Click(object sender, EventArgs e)
        {
            // initialize
            _objectManager = new ObjectManager(dateTimePickerFrom.Value, dateTimePickerTo.Value, Convert.ToInt32(textBoxParkingsCount.Text));

            //load 
            ExcelService.LoadData(textBoxWorkPath.Text, _objectManager);
            XmlService.LoadParkingCoordinates(_objectManager);
            XmlService.LoadTgoObjects(_objectManager);

            //_objectManager.ScheduleRows = _objectManager.GetScheduleRows();

            // linking
            LinkingService.LinkScheduleRowsToParkings(_objectManager);
            LinkingService.LinkTgoToScheduleRows(_objectManager);

            // processing 
            // first step generate blocks
            ParkingBlockService.FillParkingBlocks(_objectManager, Convert.ToInt32(textBoxParkingsCount.Text));

            // second handle blocks return gse count by types
            try
            {
                // Compute
                ProcessingService.HandleBlocks(_objectManager);

                // Write report
                ReportService.WriteReportInExcel(_objectManager);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message);
            }

            MessageBox.Show("Отчёт успешно сформирован!");
        }

        

        #region Form Events

        /// <summary>
        /// Обработка нажатия на кнопку выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
            //XmlService.GenerateTgoObjectsToXml();
        }

        /// <summary>
        /// Обработка нажатия на кнопку выбрать файл с расписанием
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectShedule_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxWorkPath.Text = openFileDialog.FileName;
            }
        }

        #endregion
    }
}
