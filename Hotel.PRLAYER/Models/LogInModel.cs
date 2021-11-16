using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.PRLAYER.Models
{
    public class LogInModel
    {
        [Display(Name = "Электронный адресс")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        
        
        
        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}