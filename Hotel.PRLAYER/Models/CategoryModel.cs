using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.BLogicLayer.DTO;

namespace Hotel.PRLAYER.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(40, MinimumLength = 10)]
        public String CategoryName { get; set; }
        
        
        [Required]
        [Range(1, 5)]
        public Int32 BedCount { get; set; }

        public virtual ICollection<RoomModel> Room { get; set; } = new List<RoomModel>();
        public virtual ICollection<CategoryDateModel> CategoryDate { get; set; } = new List<CategoryDateModel>();
    }
}