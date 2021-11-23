using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Exceptions;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;

namespace Hotel.BLogicLayer.Services
{
    public class BookinAndCheckInService : IBookingAndCheckInService
    {
        private IStayService _stayService;
        private IRoomService _roomService;
        
        public BookinAndCheckInService(IStayService stayService, IRoomService roomService)
        {
            _stayService = stayService;
            _roomService = roomService;
        }


        
        public void CheckOut(Guid roomId, DateTime checkOutDate)
        {
            
            var checkin = _stayService.GetCheckIns(
                stay => stay.StartDate >= checkOutDate
                        && stay.EndDate <= checkOutDate && stay.RoomId == roomId).LastOrDefault();
            if (checkin == null)
            {
                throw new ArgumentException("Not found checkIn On date");
            }

            checkin.CheckedOut = true;
            
            
            //Update checkOut date for case if person check out earilier
            if (checkin.EndDate.Day - checkOutDate.Day != 0)
            {
                checkin.EndDate = checkOutDate;
            }

            _stayService.UpdateStay(checkin);

        }
        public void CreateBooking(Guid guestId, Guid roomId, DateStartEndPair date)
        {
            CreateStayDependOnHotelPromptInfo(guestId, roomId, new HotelPromptInfo
            {
                Date = date,
                IsCheckIn = false
            });
        }

        public void CreateCheckIn(Guid guestId, Guid roomId, DateStartEndPair date)
        {
            CreateStayDependOnHotelPromptInfo(guestId, roomId,
                new HotelPromptInfo
                {
                    Date = date,
                    IsCheckIn = true
                });
        }

        private void CreateStayDependOnHotelPromptInfo(Guid guestId, Guid roomId, HotelPromptInfo info)
        {
            if (CanMakeBookingOrCheckIn(info.Date))
            {
                var stay = new StayDto
                {
                    Id = Guid.NewGuid(),
                    CheckedIn = info.IsCheckIn,
                    CheckedOut = false,
                    StartDate = info.Date.DateStart,
                    EndDate = info.Date.DateEnd,
                    GuestId = guestId,
                    RoomId = roomId
                };
                
                _stayService.CreateStay(stay);
            }

            var bookOrChekIn = info.IsCheckIn ? "check in" : "booking";
            throw new StayAreAlreadyExistException(
                $"Can't make {bookOrChekIn} on date: {info.Date.DateStart} : {info.Date.DateEnd}" );
        }
        
        

        

        public bool CanMakeBookingOrCheckIn(DateStartEndPair pair)
        {
            var canMakeBooking = !_stayService.GetStays().Any(stay => stay.StartDate == pair.DateStart && stay.EndDate == pair.DateEnd);
            return canMakeBooking;
        }

        public void Dispose()
        {
            _stayService?.Dispose();
        }
        
        public IEnumerable<RoomDto> GetFreeRoomsOnDate(DateStartEndPair dateStartEndPair)
        {
            var bookedRooms = _stayService.GetRoomsBookedOnDate(dateStartEndPair);
            var allRooms = _roomService.GetRooms();
            return allRooms.Except(bookedRooms, new RoomsComparer());
        }
        
        
        
    }
} 