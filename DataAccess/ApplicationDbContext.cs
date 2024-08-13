using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeRental> FeeRentals { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<RentalAmenity> RentalAmenities { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
    }
}
