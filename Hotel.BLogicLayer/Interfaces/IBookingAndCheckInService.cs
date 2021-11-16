using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IBookingAndCheckInService : IDisposable

    {

        public void CheckOut(Guid roomId, DateTime checkOutDate);

        public void CreateBooking(Guid guest, Guid room, DateStartEndPair date);

        public void CreateCheckIn(Guid guest, Guid room, DateStartEndPair date);

        public IEnumerable<RoomDto> GetFreeRoomsOnDate(DateStartEndPair dateStartEndPair);
        public bool CanMakeBookingOrCheckIn(DateStartEndPair pair);
    }
}