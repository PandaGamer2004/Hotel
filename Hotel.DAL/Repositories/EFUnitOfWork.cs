using Hotel.DAL.Interfaces;
using Hotel.DAL.Models;
using Hotel.DAL.Сontexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.DAL.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private HotelContext _db;

       
        public EfUnitOfWork(HotelContext context)
        {
            this._db = context;
        }
        public IGenereicRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null) _categoryRepository = new CategoryRepository(_db);
                return _categoryRepository;
            }  
        }

        public IGenereicRepository<CategoryDate> CategoryDates {
            get
            {
                if (_categoryDateRepository == null) _categoryDateRepository = new CategoryDateRepository(_db);
                return _categoryDateRepository;
            }

        }


        public IGenereicRepository<Guest> Guests {
            get
            {
                if (_guestRepository == null) _guestRepository = new GuestRepository(_db);
                return _guestRepository;
            }
        }

        public IGenereicRepository<Room> Rooms
        {
            get
            {
                if (_roomRepository == null) _roomRepository = new RoomRepository(_db);
                return _roomRepository;
            }
        }

        public IGenereicRepository<Stay> Stays
        {
            get
            {
                if (_stayRepository == null) _stayRepository = new StayRepository(_db);
                return _stayRepository;
            }
        }

        public IGenereicRepository<Role> Roles
        {
            get
            {
                if (_roleRepository == null) _roleRepository = new RoleRepository(_db);
                return _roleRepository;
            }
        }

  
        private bool disposed = false;

        private GuestRepository _guestRepository;
        private RoomRepository _roomRepository;
        private CategoryRepository _categoryRepository;
        private CategoryDateRepository _categoryDateRepository;
        private StayRepository _stayRepository;
        private RoleRepository _roleRepository;

        public void Dispose()
        {
            if(!disposed)
            {
                _db.Dispose();
            }
            
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
