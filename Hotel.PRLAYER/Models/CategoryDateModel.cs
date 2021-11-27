using System;
using Hotel.BLogicLayer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hotel.PRLAYER.Models
{
    public class CategoryDateModel
    {
        public Guid Id { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public Decimal Price { get; set; }
        
        public Guid CategoryId { get; set; }

        [BindNever]
        public virtual CategoryModel Category { get; set; }
    }
}