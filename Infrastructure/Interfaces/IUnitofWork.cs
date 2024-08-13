using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    internal interface IUnitofWork
    {
        public DbSet<Amenity> Amenity { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Fee> Fee { get; set; }
        public DbSet<FeeRental> FeeRental { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<RentalAmenity> RentalAmenity { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
    }
}
