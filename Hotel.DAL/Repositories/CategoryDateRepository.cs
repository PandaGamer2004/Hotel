
using System;
using System.Linq;
using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;

namespace Hotel.DAL.Repositories
{
    public sealed class CategoryDateRepository : GenericRepository<CategoryDate>
    {
        public CategoryDateRepository(HotelContext ctx)
        {
            _dbSet = ctx.CategoryDates;
            _context = ctx;
        }
        
    }
}

