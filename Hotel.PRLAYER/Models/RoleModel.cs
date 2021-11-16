using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.PRLAYER.Models
{
    public class RoleModel
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public String RoleName { get; set; }
    }
}