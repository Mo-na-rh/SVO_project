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
            _objectManager = new ObjectManager();
            
            openFileDialog.Filter = "Все файлы Excel (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Выбрать";
        }

        private void ComputeButton_Click(object sender, EventArgs e)
        {
            ExcelService.LoadData(textBoxWorkPath.Text, _objectManager);
            //ProcessingService.LinkTgoToScheduleRows(_objectManager);

            // replace calling xml service
            XmlService.FillCoordinatesFromXml(_objectManager);

            XmlService.FillTgoObjects(_objectManager);

            // Заполняются блоки по 3 парковочных места в одном заглушка
            ProcessingService.FillParkingBlocks(_objectManager);

            ProcessingService.LinkScheduleRowsToParkings(_objectManager);
            ProcessingService.CheckParkingBlocks(_objectManager);

            //ReportService.WriteTestResultReport(@"D:\chetv_va\Диплом 2021\Данные для работы\Results.txt", _objectManager);
            ReportService.WriteResultReportExcel(textBoxResFilePath.Text, _objectManager);

            MessageBox.Show("Отчёт успешно записан!");
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
