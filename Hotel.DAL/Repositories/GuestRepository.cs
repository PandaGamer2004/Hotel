using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.DAL.Repositories
{
    public sealed class GuestRepository : GenericRepository<Guest>
    {
        public GuestRepository(HotelContext ctx)
        {
            _dbSet = ctx.Guests;
            _context = ctx;
        }

        
    }
}
