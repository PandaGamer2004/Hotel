using System;
using System.Collections.Generic;

namespace Hotel.BLogicLayer.DTO
{
    public class CategoryDateDto
    {
        public Guid Id { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public Decimal Price { get; set; }
        
        public Guid CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var objToCategoryDate = obj as CategoryDateDto;
            if (objToCategoryDate == null) return false;
            return objToCategoryDate.Id == Id && objToCategoryDate.Price == Price
                                              && objToCategoryDate.StartDate == StartDate &&
                                              objToCategoryDate.EndDate == EndDate &&
                                              objToCategoryDate.CategoryId == CategoryId
                                              && objToCategoryDate.Category == Category;
        }

        public override int GetHashCode()
        {
            var res = 17;
            res = 31 * res + Id.GetHashCode();
            res = 31 * res + StartDate.GetHashCode();
            res = 31 * res + EndDate.GetHashCode();
            res = 31 * res + Price.GetHashCode();
            res = 31 * res + CategoryId.GetHashCode();

            return res;
        }
    }
    
}