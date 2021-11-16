using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hotel.DAL.Models
{
    public class Room
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Int32 RoomNumber { get; set; }
            
        public Guid CategoryId { get; set; }
        
        public virtual Category Category { get; set; }

        public virtual ICollection<Stay> Stays { get; set; } = new List<Stay>();
    }
}
