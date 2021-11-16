using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork _databaseUnitOfWork;
        private IMapperItem _mapperItem;

        public RoomService(IUnitOfWork databaseUnitOfWork, IMapperItem mapperItem)
        {
            _databaseUnitOfWork = databaseUnitOfWork;
            _mapperItem = mapperItem;
        }
        
        

        public void CreateRoom(RoomDto roomDto)
        {
            var room = _mapperItem.Mapper.Map<Room>(roomDto);
            _databaseUnitOfWork.Rooms.Create(room);
            _databaseUnitOfWork.Save();
        }

        public IEnumerable<RoomDto> GetRooms()
        {
            var rooms = _databaseUnitOfWork.Rooms.GetAll();
            return _mapperItem.Mapper.Map<IEnumerable<Room>, IEnumerable<RoomDto>>(rooms);
        }

        public RoomDto GetRoom(Guid id)
        {
            var room = _databaseUnitOfWork.Rooms.Get(id);
            return _mapperItem.Mapper.Map<RoomDto>(room);
        }

        public void DeleteRoom(Guid roomId)
        {
            _databaseUnitOfWork.Rooms.Delete(roomId);
        }

        public void UpdateRoom(RoomDto roomDto)
        {
            _databaseUnitOfWork.Rooms.Update(_mapperItem.Mapper.Map<Room>(roomDto));
            _databaseUnitOfWork.Save();
        }

        public void Dispose()
        {
            _databaseUnitOfWork?.Dispose();
        }
    }
}