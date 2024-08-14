using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Amenity> Amenity { get; }
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<Discount> Discount { get; }
        public IGenericRepository<Fee> Fee { get; }
        public IGenericRepository<FeeRental> FeeRental { get; }
        public IGenericRepository<Photo> Photo { get; }
        public IGenericRepository<Price> Price { get; }
        public IGenericRepository<Rental> Rental { get; }
        public IGenericRepository<RentalAmenity> RentalAmenity { get; }
        public IGenericRepository<Reservation> Reservation { get; }
    }
}
