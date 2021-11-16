
using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IRoomService : IDisposable
    {
        public void CreateRoom(RoomDto room);
        public IEnumerable<RoomDto> GetRooms();
        public RoomDto GetRoom(Guid id);
        public void DeleteRoom(Guid roomId);
        public void UpdateRoom(RoomDto room);
    }
}