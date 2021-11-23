using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class ReportService : IReportInfoService
    {
        private IStayService _stayService;
        private IRoomService _roomService;
        private ICategoryDateService _categoryDateService;
        
        
        public ReportService(ICategoryDateService categoryDateService, IRoomService roomService, IStayService stayService)
        {
            _categoryDateService = categoryDateService;
            _roomService = roomService;
            _stayService = stayService;
        }

        public async Task<ProfitReportDto> GetProfitReport(DateStartEndPair dateForReport)
        {
            ProfitReportDto reportModel = new();
                var mainTask = new Task(() =>
                {
                    var stays = _stayService.GetStays(
                        filter: stay => dateForReport.DateStart <= stay.StartDate
                                        && dateForReport.DateEnd >= stay.EndDate
                                        );
                    
                    var taskFactory = new TaskFactory(
                        CancellationToken.None,
                        TaskCreationOptions.AttachedToParent,
                        TaskContinuationOptions.ExecuteSynchronously,
                        TaskScheduler.Default);

                    taskFactory.StartNew(() =>
                    {
                        var totalPrice = stays
                            .Join(_roomService.GetRooms(), stay => stay.RoomId, room => room.Id,
                                (stay, room) => new {CategoryId = room.CategoryId})
                            .Join(_categoryDateService.GetCategoryDates(), category => category.CategoryId,
                                categoryDate => categoryDate.Id,
                                (c, cd) => new
                                {
                                    Price = cd.Price,
                                    cd.StartDate,
                                    cd.EndDate
                                })
                            .Where(item => item.StartDate >= dateForReport.DateStart
                                           && item.EndDate <= dateForReport.DateEnd)
                            .Select(item => item.Price * (item.EndDate - item.StartDate).Value.Days)
                            .Sum();
                        
                        reportModel.TotalMoneyEarned = totalPrice;
                    });
                    taskFactory.StartNew(() =>
                    {
                        var roomsInUse = stays.Select(stay => stay.RoomId).Distinct().Count();
                        reportModel.TotalRoomsInUseCount = roomsInUse;
                    });
                    taskFactory.StartNew(() =>
                    {
                        var guestsServedCount = stays.Select(stay => stay.GuestId).Distinct().Count();
                        reportModel.GuestsServed = guestsServedCount;
                    });
                    taskFactory.StartNew(() =>
                    {
                        var startDays = 0;
                        
                        var daysFree = dateForReport.DateEnd - dateForReport.DateStart;
                        
                        SortedDictionary<DateTime, String> dict = new();
                        foreach (var (dateStart, dateEnd) in stays)
                        {
                            dict[dateStart] = "start";
                            dict[dateEnd] = "end";
                        }

                        DateTime dateToRemove = default(DateTime);
                        foreach (var date in dict)
                        {
                            if (date.Value == "start")
                            {
                                startDays++;
                                if (dateToRemove == default(DateTime))
                                {
                                    dateToRemove = date.Key;
                                }
                            }
                            else if (date.Value == "end") startDays--;
                            if (startDays == 0)
                            {
                                var daysNotFree = date.Key - dateToRemove;
                                daysFree -= daysNotFree;
                            }   
                        }

                        var freeDays = daysFree.Days;
                        reportModel.DaysThatRoomsWasFree = freeDays;
                    });
                    

                });
                await mainTask;

                return await Task.FromResult(reportModel);
        }
    }
}