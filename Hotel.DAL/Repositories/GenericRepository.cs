using Hotel.DAL.Interfaces;
using Hotel.DAL.Сontexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Hotel.DAL.Repositories
{
    public abstract class GenericRepository<T> : IGenereicRepository<T> where T : class
    {
        protected DbSet<T> _dbSet;
        protected DbContext _context;

       
        public virtual void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(Guid id)
        {
            T entity = Get(id);

            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public IEnumerable<T> GetAll(string includeProperties = "",
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, 
            Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderby != null)
            {
                return orderby(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        

        public T Get(Guid id)
        { 
            var entity = _dbSet.Find(id);
            return entity;
        }

        

        public void Update(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
