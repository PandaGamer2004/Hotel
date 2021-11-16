using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.BLogicLayer.DTO
{
    public class GuestRegisterInfoDto
    {
        [Required]
        public String Login { get; set; }
        
        [Required]
        public String UserName { get; set; }
        
        [Required]
        public String Password { get; set; }
        
        
        public Guid RoleId { get; set; }
        public virtual RoleDto Role { get; set; }
    }
}