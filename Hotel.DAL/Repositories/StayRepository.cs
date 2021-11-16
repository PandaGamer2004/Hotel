using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.DAL.Repositories
{
    public class StayRepository : GenericRepository<Stay>
    {
        public StayRepository(HotelContext ctx)
        {
            _dbSet = ctx.Stays;
            _context = ctx;
        }

        
    }
}
