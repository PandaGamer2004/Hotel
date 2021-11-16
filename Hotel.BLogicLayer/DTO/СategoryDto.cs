using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hotel.DAL.Models;


namespace Hotel.BLogicLayer.DTO
{
    public class CategoryDto
    {
        
        public Guid Id { get; set; }
        
       
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 10)]
        public String CategoryName { get; set; }
        
        
        [Required]
        [Range(1, 5)]
        public Int32 BedCount { get; set; }

        public virtual ICollection<RoomDto> Room { get; set; } = new List<RoomDto>();
        public virtual ICollection<CategoryDateDto> CategoryDate { get; set; } = new List<CategoryDateDto>();
    }
    

}