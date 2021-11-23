using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hotel.PRLAYER.Models
{
    public class GuestModel
    {
        [BindNever]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Введите имя")]
        public String FirstName { get; set; }
        
        [Required(ErrorMessage = "Введите фамилию")]
        public String LastName { get; set; }
        
        [Required(ErrorMessage = "Введите отчество")]
        public String Patronimic { get; set; }
        
        
        [Required(ErrorMessage = "Введите дату рождения")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "Введите номер пасспорта")]
        [StringLength(9, MinimumLength = 9)]
        public String PassportNumber { get; set; }

        
        
        public virtual GuestRegisterInfoModel GuestRegisterInfo { get; set; }
        
        [BindNever]
        public virtual ICollection<StayModel> Stays { get; set; } = new List<StayModel>();
    }
    
}