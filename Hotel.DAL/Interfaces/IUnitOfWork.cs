using Hotel.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenereicRepository<Category> Categories { get; }
        public IGenereicRepository<CategoryDate> CategoryDates { get; }
        public IGenereicRepository<Guest> Guests { get; }
        public IGenereicRepository<Room> Rooms { get; }
        public IGenereicRepository<Stay> Stays { get; }
        public IGenereicRepository<Role> Roles { get; }

        public void Save();
        
        

    }

}
