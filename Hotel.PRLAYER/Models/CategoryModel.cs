using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.BLogicLayer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hotel.PRLAYER.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public String CategoryName { get; set; }
        
        
        [Required]
        [Range(1, 5)]
        public Int32 BedCount { get; set; }


        [BindNever]
        public virtual ICollection<RoomModel> Room { get; set; } = new List<RoomModel>();

        [BindNever]
        public virtual ICollection<CategoryDateModel> CategoryDate { get; set; } = new List<CategoryDateModel>();
    }
}