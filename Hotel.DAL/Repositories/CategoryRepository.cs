using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.DAL.Repositories
{
    public sealed class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(HotelContext context)
        {
            _dbSet = context.Categories;
            _context = context;
        }
        
    }
}
