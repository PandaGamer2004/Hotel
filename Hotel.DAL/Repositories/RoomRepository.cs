
using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;
using System;
using System.Linq;

namespace Hotel.DAL.Repositories
{

    public sealed class RoomRepository : GenericRepository<Room>{
        public RoomRepository(HotelContext ctx)
        {
            _dbSet = ctx.Rooms;
            _context = ctx;
        }

        
    }

    
}
