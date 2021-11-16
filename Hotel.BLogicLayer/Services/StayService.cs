using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.BuisnessLogic;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class StayService : IStayService
    {
        private IMapperItem _mapperItem;
        private IUnitOfWork _dbUnitOfWork;

        public StayService(IMapperItem mapperItem, IUnitOfWork dbUnitOfWork)
        {
            _mapperItem = mapperItem;
            _dbUnitOfWork = dbUnitOfWork;
        }

        public IEnumerable<RoomDto> GetRoomsBookedOnDate(DateStartEndPair dateStartEndPair)
        {
            var roomsBookedOnDate = _dbUnitOfWork.Stays.GetAll("Room",
                filter: stay =>
                    (dateStartEndPair.DateStart >= stay.StartDate && dateStartEndPair.DateStart <= stay.EndDate)
                    || (dateStartEndPair.DateEnd >= stay.StartDate && dateStartEndPair.DateEnd <= stay.EndDate)
            ).Select(stay => stay.Room);
            return _mapperItem.Mapper.Map<IEnumerable<Room>, IEnumerable<RoomDto>>(roomsBookedOnDate);
        }

        public void CreateStay(StayDto stayDto)
        {
            if (stayDto.Guest == null || stayDto.Room == null)
            {
                throw new ArgumentException("Can't create stay without guest or room");
            }
            
            var stay = _mapperItem.Mapper.Map<Stay>(stayDto);
            _dbUnitOfWork.Stays.Create(stay);
            _dbUnitOfWork.Save();
        }

        public IEnumerable<StayDto> GetBookings(Predicate<Stay> filterBookings = null)
        {
            return GetStays(stay => !stay.CheckedIn && (filterBookings == null || filterBookings(stay)));
        }

        public IEnumerable<StayDto> GetCheckIns(Predicate<Stay> filterCheckIns = null)
        {
            return GetStays(stay => stay.CheckedIn && (filterCheckIns == null || filterCheckIns(stay)));
        }
        
        public IEnumerable<StayDto> GetStays(Predicate<Stay> filterStays = null)
        {
            return _mapperItem.Mapper.Map<IEnumerable<Stay>, IEnumerable<StayDto>>(
                _dbUnitOfWork.Stays.GetAll(
                    filter: stay => filterStays == null || filterStays(stay)));
            
        }

        public StayDto GetStay(Guid id)
        {
            var stay = _dbUnitOfWork.Stays.Get(id);
            return _mapperItem.Mapper.Map<StayDto>(stay);
        }

        public void DeleteStay(Guid stayId)
        {
            _dbUnitOfWork.Stays.Delete(stayId);
            _dbUnitOfWork.Save();
        }

        public void UpdateStay(StayDto stay)
        {
            var stayFromDto = _mapperItem.Mapper.Map<Stay>(stay);
            _dbUnitOfWork.Stays.Update(stayFromDto);
            _dbUnitOfWork.Save();
        }

        public void Dispose()
        {
            _dbUnitOfWork?.Dispose();
        }
    }
}