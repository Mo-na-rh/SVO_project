using System;
using System.Collections.Generic;
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
        /// Хочу сгенерить 100 объектов ТГО в xml формате
        /// </summary>
        public static void GenerateTgoObjectsToXml()
        {

            var path = "D:\\Chetverikov\\TGOs.xml";

            XDocument xdoc = new XDocument();

            // создаем корневой элемент
            XElement TGOs = new XElement("TGOs");

            var acDict = GetAcDict();

            for (int i = 1; i < 165; i++)
            {
                var type = "";
                var typePlane = "";
                var airCompany = "";

                if (i < 83)
                {
                    type = "Вылет";

                    if (i < 42)
                    {
                        typePlane = "ШФ";
                        airCompany = acDict[i];
                    }
                    else
                    {
                        typePlane = "УФ";
                        airCompany = acDict[i-41];
                    }
                }
                else
                {
                    type = "Прилет";

                    if (i < 124)
                    {
                        typePlane = "ШФ";
                        airCompany = acDict[i - 82];
                    }
                    else
                    {
                        typePlane = "УФ";
                        airCompany = acDict[i - 123];
                    }

                }

                XElement tgo = new XElement("TGO");
             
                XAttribute id = new XAttribute("id", i);
                XElement airCompanyTGO = new XElement("airCompany", airCompany);
                XElement totalTimeTGO = new XElement("totalTime", "00:60");
                XElement typePlaneTGO = new XElement("typePlane", typePlane);
                XElement typeTGO = new XElement("type", type);

                XElement operations = new XElement("operations");
                FillOperations(operations, type);

                // Добавление аттрибута id
                tgo.Add(id);

                tgo.Add(airCompanyTGO);
                tgo.Add(totalTimeTGO);
                tgo.Add(typePlaneTGO);
                tgo.Add(typeTGO);

                tgo.Add(operations);

                TGOs.Add(tgo);
            }

            // добавляем корневой элемент в документ
            xdoc.Add(TGOs);
            //сохраняем документ
            xdoc.Save(path);
        }

        /// <summary>
        /// Сделать потом отдельно для отправлений, прибытий шф/уф
        /// </summary>
        /// <param name="operations"></param>
        private static void FillOperations(XElement operations, string type)
        {
            var operationArrival = GetOperation("Прибытие ВС", "Водила", "00:00", "00:00");
            var operation1 = GetOperation("Установка колодок", "Колодки", "00:00", "00:05");
            var operation2 = GetOperation("Установка конусов безопасности", "Конуса безопасности", "00:05", "00:10");
            var operation3 = GetOperation("Подгон первого трапа", "Трапы", "00:04", "00:06");
            var operation4 = GetOperation("Открытие грузовых люков", "Стремянки", "00:08", "00:10");
            var operation5 = GetOperation("Закрытие грузовых люков", "Стремянки", "00:42", "00:45");
            var operation6 = GetOperation("Отгон первого трапа", "Трапы", "00:45", "00:48");
            var operation7 = GetOperation("Уборка колодок", "Колодки", "00:40", "00:45");
            var operation8 = GetOperation("Уборка конусов безопасности", "Конуса безопасности", "00:45", "00:50");       
            var operationDeparture = GetOperation("Отправление ВС", "Водила", "00:00", "00:00");




            operations.Add(operationArrival);
            operations.Add(operation1);
            operations.Add(operation2);
            operations.Add(operation3);
            operations.Add(operation4);
            operations.Add(operation5);
            operations.Add(operation6);
            operations.Add(operation7);
            operations.Add(operation8);
            if (type == "Вылет")
                operations.Add(operationDeparture);
        }

        private static XElement GetOperation(string name, string gseType, string start, string end)
        {
            var operation = new XElement("operation");
            var nameEl = new XElement("name", name);
            var gseTypeEl = new XElement("gseType", gseType);
            var startTimeEl = new XElement("startTime", start);
            var endTimeEl = new XElement("endTime", end);

            operation.Add(nameEl);
            operation.Add(gseTypeEl);
            operation.Add(startTimeEl);
            operation.Add(endTimeEl);

            return operation;
        }

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
                    tgo.AircompanyCode = element.Element("airCompany")?.Value;
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

        private static Dictionary<int, string> GetAcDict()
        {
            return new Dictionary<int, string>
            {
                {1,"N4"},
                {2,"SU"},
                {3,"EO"},
                {4,"FV"},
                {5,"BT"},
                {6,"OK"},
                {7,"KC"},
                {8,"O"},
                {9,"AY"},
                {10,"CZ"},
                {11,"U6"},
                {12,"KE"},
                {13,"CA"},
                {14,"K"},
                {15,"MU"},
                {16,"AZ"},
                {17,"BA"},
                {18,"D2"},
                {19,"AF"},
                {20,"AH"},
                {21,"8"},
                {22,"JU"},
                {23,"KM"},
                {24,"R"},
                {25,"HU"},
                {26,"3U"},
                {27,"QS"},
                {28,"FB"},
                {29,"OM"},
                {30,"JD"},
                {31,"6Q"},
                {32,"L"},
                {33,"FG"},
                {34,"YC"},
                {35,"B2"},
                {36,"GS"},
                {37,"ZF"},
                {38,"E9"},
                {39,"OS"},
                {40,"E"},
                {41,"8Q"}
            };
        }
    }
}