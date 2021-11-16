using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Hotel.DAL.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
   

        [MaxLength(20)]
        [Required]
        public String CategoryName { get; set; }
        
        public Int32 BedCount { get; set; }


        public virtual ICollection<Room> Room { get; set; } = new List<Room>();

        public virtual ICollection<CategoryDate> CategoryDate { get; set; } = new List<CategoryDate>();
    }
}
