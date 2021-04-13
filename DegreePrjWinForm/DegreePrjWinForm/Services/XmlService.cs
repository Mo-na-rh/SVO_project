using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Managers;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис для работы с Xml
    /// </summary>
    public static class XmlService
    {
        public static string GetPathToXml()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.IndexOf("bin"));
            return path + @"Source\Xml\";
        }

        /// <summary>
        /// Зачитывание координат парковочных мест из xml файлов
        /// </summary>
        public static void FillCoordinatesFromXml(ObjectManager objMgr)
        {
            //var pathToFile = @"C:\Users\chetv_va\Desktop\Education\Diploma\Git\Degree-project\DegreePrjWinForm\DegreePrjWinForm\Source\Xml\Parkings\";
            var pathToFile = GetPathToXml() + @"Parkings\";

            foreach (var o in objMgr.Parkings)
            {
                var path = pathToFile + o.Number + ".xml";
                try
                {
                    var xdoc = XDocument.Load(path);
                    var geozoneType = xdoc.Element("geozoneType");

                    var geometry = geozoneType.Elements("geometry").FirstOrDefault();
                    foreach (var element in geometry.Elements("point"))
                    {
                        var nameX = element.Attribute("x");
                        var nameY = element.Attribute("y");
                        if (nameX == null || nameY == null) continue;
                        var coordObj = new Coordinate();
                        var englishCulture = CultureInfo.GetCultureInfo("en-US");
                        coordObj.X = double.Parse(nameX.Value, englishCulture);
                        coordObj.Y = double.Parse(nameY.Value, englishCulture);
                        o.Coordinates.Add(coordObj);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    //_logger.Trace("Файл не найден!" + ex.Message);
                }
            }
        }

        public static void FillTgoObjects(ObjectManager objMgr)
        {
            //var path = @"D:\chetv_va\Диплом 2021\Данные для работы\Xml\Tgo\1.xml";
            var path = GetPathToXml() + @"Tgo\Tgo.xml"; ;
            try
            {
                var xdoc = XDocument.Load(path);
                foreach (var element in xdoc.Elements("TGOs"))
                {
                    var tgo = new TGO();

                    tgo.AirCompanyCode = element.Attribute("name")?.Value;
                    tgo.TotalTime = element.Attribute("totalTime")?.Value;
                    tgo.Type = GetTgoType(element.Attribute("type")?.Value);
                    // заполнить базовые параметры

                    var operations = element.Elements("operations").FirstOrDefault();
                    foreach (var xElement in operations.Elements("operation"))
                    {
                        var operation = new Operation();
                        operation.Name = xElement.Attribute("name")?.Value;
                        operation.GseType = xElement.Attribute("gseType")?.Value;
                        operation.StartTime = xElement.Attribute("startTime")?.Value;
                        operation.EndTime = xElement.Attribute("endTime")?.Value;

                        tgo.Operations.Add(operation);
                    }

                    objMgr.TgoObjects.Add(tgo);
                }

            }
            catch (FileNotFoundException ex)
            {
                //    _logger.Trace("Файл не найден!" + ex.Message);
            }
        }
        private static TgoType GetTgoType(string value)
        {
            if (value.Contains("Прилет"))
                return TgoType.arrival;

            if (value.Contains("Вылет"))
                return TgoType.departure;

            return TgoType.reverse;
        }
    }
}