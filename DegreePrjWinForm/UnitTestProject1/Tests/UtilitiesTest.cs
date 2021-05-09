using System;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void TryParsePath()
        {
            var path = @"C:\Users\chetv_va\Desktop\Education\Diploma\Git\Degree-project\DegreePrjWinForm\DegreePrjWinForm\bin\Debug\DegreePrjWinForm.exe";
            path = path.Substring(0, path.IndexOf("bin"));
            path = path + @"Source\Xml\Parkings";
            var originPath = @"C:\Users\chetv_va\Desktop\Education\Diploma\Git\Degree-project\DegreePrjWinForm\DegreePrjWinForm\Source\Xml\Parkings";
            Assert.AreEqual(originPath, path);
        }

        [TestMethod]
        public void CheckGetFlightStartDateFromRow()
        {
            var row = new ScheduleRow();
            DateTime date1 = new DateTime(2020, 5, 1);
            row.FlightDate = date1.ToString();
            DateTime date2 = new DateTime(1989, 1, 1, 10, 35, 0);
            row.FlightScheduleTime = date2.ToString();

            var resultDate = row.GetStartDate();

            DateTime date3 = new DateTime(2020, 5, 1, 10, 35, 0);
            Assert.AreEqual(date3, resultDate);
        }

        [TestMethod]
        public void TryParceDateTime()
        {
            var dateStr = "60:00";
            var test1 = dateStr.Split(':');
            var time = TimeSpan.FromMinutes(int.Parse(test1[0]));
            var resultDate = new DateTime(1989, 1, 1, 0, 0, 0) + time;

            DateTime date3 = new DateTime(1989, 1, 1, 1, 0, 0);

            Assert.AreEqual(date3, resultDate);
        }

        [TestMethod]
        public void TryCleeDateAndTIme()
        {
           // var a = DateTime.Now;
           // var minutes = "00:08";
           //var b = a + ProcessingService.GetTimeSpanFromMinutes(minutes);
        }
    }
}
