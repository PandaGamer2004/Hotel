using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.BLogicLayer.DTO;

namespace Hotel.PRLAYER.Models
{
    public class RoomModel
    {
        
        public Guid Id { get; set; }
        
        
        [Required(ErrorMessage = "Введите номер комнаты")]
        [Display(Name="Номер комнаты")]
        [Range(1, 1000)]
        public Int32 RoomNumber { get; set; }
        
         
        public Guid CategoryId { get; set; }
        
        public virtual CategoryModel Category { get; set; }
        public virtual ICollection<StayModel> Stays { get; set; } = new List<StayModel>();
        
    }
}