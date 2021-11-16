using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.DAL.Models;

namespace Hotel.BLogicLayer.DTO
{
    
    
    public class GuestDto
    {
        
        public Guid Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Patronimic { get; set; }
        public DateTime BirthDate { get; set; }
        public String PassportNumber { get; set; }
        
        
        
        [Required]
        public virtual GuestRegisterInfoDto GuestRegisterInfo { get; set; }
        
        public virtual ICollection<StayDto> Stays { get; set; } = new List<StayDto>();
    }
}