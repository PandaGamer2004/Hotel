using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.DAL.Models
{
    [Owned]
    public class GuestRegisterInfo
    {
        [Required]
        public String Login { get; set; }
        
        [Required]
        public String UserName { get; set; }
        
        [Required]
        public String Password { get; set; }
        
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        
        
        public virtual Role Role { get; set; }
    }
}