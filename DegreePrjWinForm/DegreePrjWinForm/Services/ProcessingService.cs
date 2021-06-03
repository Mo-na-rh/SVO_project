using System.Linq;
using DegreePrjWinForm.Classes;
using DegreePrjWinForm.Enums;
using DegreePrjWinForm.Managers;

namespace DegreePrjWinForm.Services
{
    /// <summary>
    /// Сервис по работе с объектами моделирования
    /// </summary>
    public static class ProcessingService
    {
        /// <summary>
        /// Обработка блоков, расчёт количеств СНО по блокам
        /// </summary>
        /// <param name="objectManager"></param>
        public static void HandleBlocks(ObjectManager objectManager)
        {
            SetStartAndFinishTgo(objectManager);

            foreach (var block in objectManager.ParkingBlocks)
            {
                HandleBlock(block);
            }
        }

        /// <summary>
        /// Основной метод здесь расчитывается количество СНО на 1 блок МС на заданный период времени
        /// </summary>
        /// <param name="block"></param>
        private static void HandleBlock(ParkingBlock block)
        {
            block.PredictGseDemandForAllFlightsInBlock();

            block.BlockGseCount = block.GetGseCountByType(GseType.block);
            block.LadderGseCount = block.GetGseCountByType(GseType.ladder);
            block.MarkerConeGseCount = block.GetGseCountByType(GseType.markerCone);
            block.TowBarGseCount = block.GetGseCountByType(GseType.towBar);
        }


        /// <summary>
        /// Установка начала и конца ТГО по строкам расписания
        /// </summary>
        /// <param name="objectManager"></param>
        private static void SetStartAndFinishTgo(ObjectManager objectManager)
        {
            foreach (var row in from block in objectManager.ParkingBlocks from parking in block.Parkings from row in parking.LinkedScheduleRows select row)
            {
                row.StartTGO = row.GetStartTGODate();

                if (row.LinkedTGO.Type == TgoType.departure)
                {
                    // Если отправление отнимаем
                    row.EndTGO = row.StartTGO;
                    row.StartTGO = row.StartTGO.Subtract(row.LinkedTGO.GetTotalTime().TimeOfDay);
                }
                else
                {
                    // если прибытие прибавляем
                    row.EndTGO = row.StartTGO.Add(row.LinkedTGO.GetTotalTime().TimeOfDay);
                }
            }
        }

    }
}
