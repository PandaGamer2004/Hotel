using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hotel.DAL.Interfaces
{
    public interface IGenereicRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(
            string includeProperties = "",
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, 
            Expression<Func<T, bool>> filter = null
        );
        public T Get(Guid id);
        public void Create(T entity);
        public void Delete(Guid id);
        public void Update(T entity);


    }
}
