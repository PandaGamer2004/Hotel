using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.DTO;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IStayService : IDisposable
    {
        public void CreateStay(StayDto stayDto);
        public IEnumerable<StayDto> GetStays(Predicate<Stay> filter = null);
        
        public IEnumerable<RoomDto> GetRoomsBookedOnDate(DateStartEndPair dateStartEndPair);

        public IEnumerable<StayDto> GetBookings(Predicate<Stay> filter = null);
        public IEnumerable<StayDto> GetCheckIns(Predicate<Stay> filter = null);
        public StayDto GetStay(Guid id);
        public void DeleteStay(Guid stayId);
        public void UpdateStay(StayDto stay);
    }
}