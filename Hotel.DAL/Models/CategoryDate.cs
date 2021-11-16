using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.DAL.Models
{
    public class CategoryDate
    {
        

        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        
        
        [Column(TypeName = "money")]
        public Decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        
        public virtual Category Category { get; set; }
    }

}
