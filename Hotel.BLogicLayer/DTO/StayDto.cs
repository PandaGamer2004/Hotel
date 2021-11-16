using System;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.DTO
{
    public class StayDto
    {
        public Guid Id { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public Boolean CheckedIn { get; set; }
        public Boolean CheckedOut { get; set; }
        
        public Guid GuestId { get; set; }
        public Guid RoomId { get; set; }
        
        public virtual RoomDto Room { get; set; }
        public virtual GuestDto Guest { get; set; }


        public void Deconstruct(out DateTime dateStart, out DateTime dateEnd)
        {
            dateStart = StartDate;
            dateEnd = EndDate;
        }
    }
}