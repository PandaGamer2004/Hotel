using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hotel.PRLAYER.Models
{
    public class GuestRegisterInfoModel
    {
        [Required]
        public String Login { get; set; }
        
        [Required]
        public String UserName { get; set; }
        
        [Required]
        public String Password { get; set; }
        
        
        [BindNever]
        public Guid RoleId { get; set; }
        [BindNever]
        public virtual RoleModel Role { get; set; }
    }
}