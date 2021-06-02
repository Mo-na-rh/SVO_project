using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Managers;
using System.Collections.Generic;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис для генерации вариантов распределения МС по блокам
    /// </summary>
    public static class ParkingBlockService
    {
        ///// <summary>
        ///// Проверка если хотябы одно МС не пустует то Блок считать заполненным
        ///// </summary>
        ///// <param name="objectMgr"></param>
        //public static void CheckParkingBlocks(ObjectManager objectMgr)
        //{
        //    foreach (var aircraftParkingsBlock in objectMgr.ParkingBlocks)
        //    {
        //        foreach (var parkingObject in aircraftParkingsBlock.Parkings)
        //        {
        //            if (parkingObject.LinkedScheduleRows.Count <= 0) continue;
        //            aircraftParkingsBlock.IsFilled = true;
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// Заполнение блоков парковок заглушка
        /// </summary>
        /// <param name="objMgr"></param>
        public static void FillParkingBlocks(ObjectManager objMgr, int parkingCount)
        {
            objMgr.ParkingBlocks.Clear();
            var i = 1;
            var j = 1;
            var block = new ParkingBlock();
            block.Id = i;
            objMgr.ParkingBlocks.Add(block);

            foreach (var parking in objMgr.Parkings)
            {
                if (i % parkingCount != 0)
                {
                    block.Parkings.Add(parking);
                }
                else
                {
                    block = new ParkingBlock { Parkings = new List<Parking>(), Id = j+1 };
                    j++;
                    block.Parkings.Add(parking);
                    objMgr.ParkingBlocks.Add(block);
                }

                i++;
            }
        }
    }
}
