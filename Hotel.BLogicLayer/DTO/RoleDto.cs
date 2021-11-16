using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.BLogicLayer.DTO
{
    public class RoleDto
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public String RoleName { get; set; }
    }
}