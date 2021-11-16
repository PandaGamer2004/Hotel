using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hotel.DAL.Models
{
    public class Stay
    {

     
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public Boolean CheckedIn { get; set; }
        
        public Boolean CheckedOut { get; set; }

        public Guid RoomId { get; set; }
        public Guid GuestId { get; set; }
        
        public virtual Room Room { get; set; }
        
        public virtual Guest Guest { get; set; }
    }
}
