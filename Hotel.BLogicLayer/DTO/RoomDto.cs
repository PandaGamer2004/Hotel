using System;
using System.Collections.Generic;

namespace Hotel.BLogicLayer.DTO
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public Int32 RoomNumber { get; set; }
     
        
        public Guid CategoryId { get; set; }
        
        public virtual CategoryDto Category { get; set; }
        public virtual ICollection<StayDto> Stays { get; set; } = new List<StayDto>();
    }
}