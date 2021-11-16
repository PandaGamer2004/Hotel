using System;
using Hotel.BLogicLayer.DTO;

namespace Hotel.PRLAYER.Models
{
    public class CategoryDateModel
    {
        public Guid Id { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public Decimal Price { get; set; }
        
        public Guid CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
    }
}