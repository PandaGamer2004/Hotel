using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Exceptions;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.Services
{
    public class GuestService : IGuestService
    {
        private IMapperItem _contextMapper;
        private IUnitOfWork _dataBaseUnitOfWork;
        
        
        
        public GuestService(IUnitOfWork uf, IMapperItem map)
        {
            this._dataBaseUnitOfWork = uf;
            this._contextMapper = map;
        }
        public void CreateGuest(GuestDto guestDto)
        {
            var guestsWithEqualLoginCount = _dataBaseUnitOfWork.Guests.GetAll(filter:
                (guest) => guest.GuestRegisterInfo.Login == guestDto.GuestRegisterInfo.Login).Count();

            var guestsWithEqualUserNameCount = _dataBaseUnitOfWork.Guests.GetAll(
                filter: guest => guest.GuestRegisterInfo.UserName == guestDto.GuestRegisterInfo.UserName).Count();
            
            if (guestsWithEqualLoginCount != 0)
                throw new GuestEmailAlreadyExistException("Guest with current email are already exist");

            if (guestsWithEqualUserNameCount != 0)
                throw new GuestUserNameAlreadyExistException("Guest with current username are already exist");
            
            var guest = _contextMapper.Mapper.Map<Guest>(guestDto);
            _dataBaseUnitOfWork.Guests.Create(guest);
            _dataBaseUnitOfWork.Save();
        }

        public GuestDto GetGuestByUserName(string userName)
        {
            var userWithEqualUserName = _dataBaseUnitOfWork.Guests.GetAll(
                filter: guest => guest.GuestRegisterInfo.UserName == userName).LastOrDefault();

            if (userWithEqualUserName == null) throw new UserNotExistException("Not found User with Given User Name");

            return _contextMapper.Mapper.Map<GuestDto>(userWithEqualUserName);
        }

        public GuestDto GetGuestByEmailAndPassword(String login, String password)
        {
            var guest = _dataBaseUnitOfWork.Guests.GetAll(
                includeProperties: "GuestRegisterInfo",
                filter: guest => guest.GuestRegisterInfo.Login == login
                                 && guest.GuestRegisterInfo.Password == password).LastOrDefault();

            if (guest == null) throw new KeyNotFoundException("Can't found guest with given login and password");

            return _contextMapper.Mapper.Map<GuestDto>(guest);
        }

        public IEnumerable<GuestDto> GetGuests()
        {
            var guests = _dataBaseUnitOfWork.Guests.GetAll();
            return guests.Select(g  => _contextMapper.Mapper.Map<GuestDto>(g));
        }
        

        public GuestDto GetGuest(Guid id)
        {
            var guest = _dataBaseUnitOfWork.Guests.Get(id);
            return _contextMapper.Mapper.Map<GuestDto>(guest);
        }

        public void DeleteGuest(Guid guestId)
        {
            _dataBaseUnitOfWork.Guests.Delete(guestId);
            _dataBaseUnitOfWork.Save();
        }

        public void UpdateGuest(GuestDto guest)
        {
            _dataBaseUnitOfWork.Guests.Update(_contextMapper.Mapper.Map<Guest>(guest));
            _dataBaseUnitOfWork.Save();
        }

        public void Dispose()
        {
            _dataBaseUnitOfWork?.Dispose();
        }
    }
}