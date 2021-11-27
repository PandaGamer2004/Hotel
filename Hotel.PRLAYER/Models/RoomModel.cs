using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.BLogicLayer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        

        [BindNever]
        public virtual CategoryModel Category { get; set; }
        [BindNever]
        public virtual ICollection<StayModel> Stays { get; set; } = new List<StayModel>();
        
    }
}