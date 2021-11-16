using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;

namespace Hotel.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(HotelContext context)
        {
            _dbSet = context.Roles;
            _context = context;
        }
    }
}