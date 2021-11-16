using System;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.Interfaces
{
    public interface IGuestService : IDisposable
    {
        public void CreateGuest(GuestDto guestDto);
        public IEnumerable<GuestDto> GetGuests();
        public GuestDto GetGuest(Guid id);

        public GuestDto GetGuestByUserName(String username);
        public GuestDto GetGuestByEmailAndPassword(String login, String password);
        
        public void DeleteGuest(Guid guest);
        public void UpdateGuest(GuestDto guest);
    }
}