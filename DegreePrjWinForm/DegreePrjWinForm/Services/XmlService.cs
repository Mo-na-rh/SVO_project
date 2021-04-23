using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Enums;
using DegreePrjWinForm.Managers;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис для работы с Xml, заполнение мест стоянок и ТГО
    /// </summary>
    public static class XmlService
    {
        /// <summary>
        /// Получение пути к текущей папке с Xml
        /// </summary>
        /// <returns></returns>
        public static string GetPathToXml()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.IndexOf("bin"));
            return path + @"Source\Xml\";
        }

        /// <summary>
        /// Зачитывание координат парковочных мест из xml 
        /// </summary>
        public static void LoadParkingCoordinates(ObjectManager objMgr)
        {
            var pathToFile = GetPathToXml() + @"Parkings\";

            foreach (var parking in objMgr.Parkings)
            {
                var path = pathToFile + parking.Number + ".xml";
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
                        var coordinates = new Coordinate();
                        var englishCulture = CultureInfo.GetCultureInfo("en-US");
                        coordinates.X = double.Parse(nameX.Value, englishCulture);
                        coordinates.Y = double.Parse(nameY.Value, englishCulture);
                        parking.Coordinates.Add(coordinates);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    //TODO handle exception
                    //_logger.Trace("Файл не найден!" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Загрузка объектов ТГО из XML
        /// </summary>
        /// <param name="objMgr"></param>
        public static void LoadTgoObjects(ObjectManager objMgr)
        {
            var path = GetPathToXml() + @"Tgo\Tgo.xml"; ;
            try
            {
                var xdoc = XDocument.Load(path);
                var rootElement = xdoc.Element("TGOs");
                foreach (var element in rootElement.Elements("TGO"))
                {
                    var tgo = new TGO();
                    tgo.ACCode = element.Element("airCompany")?.Value;
                    tgo.TotalTime = element.Element("totalTime")?.Value;
                    var tp = element.Element("type")?.Value;
                    if (!string.IsNullOrEmpty(tp))
                    {
                        tgo.Type = GetTgoType(tp);
                    }
                    var planeType = element.Element("type")?.Value;
                    if (!string.IsNullOrEmpty(tp))
                    {
                        tgo.AircraftBodyType = GetPlaneType(planeType);
                    }

                    // заполнить базовые параметры
                    var operations = element.Elements("operations").FirstOrDefault();
                    foreach (var xElement in operations.Elements("operation"))
                    {
                        var operation = new Operation();
                        operation.Name = xElement.Element("name")?.Value;
                        operation.GseType = xElement.Element("gseType")?.Value;
                        operation.StartTime = xElement.Element("startTime")?.Value;
                        operation.EndTime = xElement.Element("endTime")?.Value;

                        tgo.Operations.Add(operation);
                    }
                    objMgr.TgoObjects.Add(tgo);
                }
            }
            catch (FileNotFoundException ex)
            {
                // TODO handle exception
                //    _logger.Trace("Файл не найден!" + ex.Message);
            }
        }

        /// <summary>
        /// Получения типа ТГО
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static TgoType GetTgoType(string value)
        {
            if (value.Contains("Прилет"))
                return TgoType.arrival;

            if (value.Contains("Вылет"))
                return TgoType.departure;

            return TgoType.reverse;
        }

        /// <summary>
        /// Получения типа ВС
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static AircraftBodyType GetPlaneType(string value)
        {
            if (value.Contains("ШФ"))
                return AircraftBodyType.wide;

            return AircraftBodyType.narrow;
        }
    }
}