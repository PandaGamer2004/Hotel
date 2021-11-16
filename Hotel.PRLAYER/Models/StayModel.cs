using System;
using System.ComponentModel.DataAnnotations;
using Hotel.BLogicLayer.DTO;

namespace Hotel.PRLAYER.Models
{
    public class StayModel
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Введите дату заезда")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "Введите дату выезда")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        public Boolean CheckedIn { get; set; } = false;
        public Boolean CheckedOut { get; set; } = false;
        
        public Guid GuestId { get; set; }
        public Guid RoomId { get; set; }
        
        public virtual RoomModel Room { get; set; }
        public virtual GuestModel Guest { get; set; }
    }
}