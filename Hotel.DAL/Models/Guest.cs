using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hotel.DAL.Models
{
    public class Guest
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public String FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public String LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public String Patronimic { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        [Required]
        [MaxLength(9)]
        [MinLength(9)]
        public String PassportNumber { get; set; }
        
        
        
        [Required]
        public virtual GuestRegisterInfo GuestRegisterInfo { get; set; }
        public virtual ICollection<Stay> Stays { get; set; } = new List<Stay>();
    }
    
    
}
