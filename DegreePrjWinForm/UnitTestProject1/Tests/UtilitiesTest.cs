using System;
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
            path = path + @"Source\Xml";
            var originPath = @"C:\Users\chetv_va\Desktop\Education\Diploma\Git\Degree-project\DegreePrjWinForm\DegreePrjWinForm\Source\Xml\Parkings";
            Assert.AreEqual(originPath, path);
        }
    }
}
