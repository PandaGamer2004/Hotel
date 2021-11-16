using Hotel.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Hotel.DAL.Сontexts
{
    public sealed class HotelContext : DbContext
    {
        public DbSet<Stay> Stays { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }

        public DbSet<Category> Categories { get; set; }
        
        public DbSet<CategoryDate> CategoryDates { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var category = new Category {Id = Guid.NewGuid(), BedCount = 3, CategoryName = "Lux With 3 Rooms"};
            var room1 = new Room
            {
                Id = Guid.NewGuid(), RoomNumber = 1
                ,CategoryId = category.Id
            };
            var room2 = new Room
            {
                Id = Guid.NewGuid(),RoomNumber = 2, 
                CategoryId = category.Id
            };
            var room3 = new Room
            {
                Id = Guid.NewGuid(), RoomNumber = 3, 
                CategoryId = category.Id
            };

            var categoryDate = new CategoryDate
            {
                Id = Guid.NewGuid(),  StartDate = DateTime.Now, EndDate = null, Price = 3000,
                CategoryId = category.Id
            };

            modelBuilder.Entity<Category>().HasData(
                category
            );

            modelBuilder.Entity<Room>().HasData(
                room1, room2, room3
            );
            modelBuilder.Entity<CategoryDate>().HasData(
                categoryDate);

            modelBuilder.Entity<Role>().HasData(
                new Role {Id = Guid.NewGuid(), RoleName = "User"});
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
