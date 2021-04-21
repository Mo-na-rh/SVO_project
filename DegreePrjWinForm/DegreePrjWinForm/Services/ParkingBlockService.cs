﻿using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Managers;
using System.Collections.Generic;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис для генерации вариантов распределения МС по блокам
    /// </summary>
    public static class ParkingBlockService
    {
        /// <summary>
        /// Проверка если хотябы одно МС не пустует то Блок считать заполненным
        /// </summary>
        /// <param name="objectMgr"></param>
        public static void CheckParkingBlocks(ObjectManager objectMgr)
        {
            foreach (var aircraftParkingsBlock in objectMgr.ParkingBlocks)
            {
                foreach (var parkingObject in aircraftParkingsBlock.AircraftParkings)
                {
                    if (parkingObject.LinkedScheduleRows.Count <= 0) continue;
                    aircraftParkingsBlock.IsFilled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Заполнение блоков парковок заглушка
        /// </summary>
        /// <param name="objMgr"></param>
        public static void FillParkingBlocks(ObjectManager objMgr)
        {
            var i = 1;
            var block = new ParkingBlock();
            block.Id = i;
            block.AircraftParkings = new List<Parking>();
            objMgr.ParkingBlocks.Add(block);
            foreach (var parkingObject in objMgr.Parkings)
            {
                if (i % 3 != 0)
                {
                    block.AircraftParkings.Add(parkingObject);
                }
                else
                {
                    block = new ParkingBlock { AircraftParkings = new List<Parking>(), Id = i };
                    block.AircraftParkings.Add(parkingObject);
                    objMgr.ParkingBlocks.Add(block);
                }

                i++;
            }
        }
    }
}