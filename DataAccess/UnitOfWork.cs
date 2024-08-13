using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;  //dependency injection of Data Source

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<Amenity> _Amenity;
        public IGenericRepository<ApplicationUser> _ApplicationUser;
        public IGenericRepository<Discount> _Discount;
        public IGenericRepository<Fee> _Fee;
        public IGenericRepository<FeeRental> _FeeRental;
        public IGenericRepository<Photo> _Photo;
        public IGenericRepository<Price> _Price;
        public IGenericRepository<Rental> _Rental;
        public IGenericRepository<RentalAmenity> _RentalAmenity;
        public IGenericRepository<Reservation> _Reservation;

        public IGenericRepository<Amenity> Amenity 
        {
            get
            {
                if (_Amenity == null)
                {
                    _Amenity = new GenericRepository<Amenity>(_dbContext);
                }
                return _Amenity;
            }
        }
        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                if (_ApplicationUser == null)
                {
                    _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
                }
                return _ApplicationUser;
            }
        }
        public IGenericRepository<Discount> Discount
        {
            get
            {
                if (_Discount == null)
                {
                    _Discount = new GenericRepository<Discount>(_dbContext);
                }
                return Discount;
            }
        }
        public IGenericRepository<Fee> Fee
        {
            get
            {
                if (_Fee == null)
                {
                    _Fee = new GenericRepository<Fee>(_dbContext);
                }
                return _Fee;
            }
        }
        public IGenericRepository<FeeRental> FeeRental
        {
            get
            {
                if (_FeeRental == null)
                {
                    _FeeRental = new GenericRepository<FeeRental>(_dbContext);
                }
                return _FeeRental;
            }
        }
        public IGenericRepository<Photo> Photo
        {
            get
            {
                if (_Photo == null)
                {
                    _Photo = new GenericRepository<Photo>(_dbContext);
                }
                return _Photo;
            }
        }
        public IGenericRepository<Price> Price
        {
            get
            {
                if (_Price == null)
                {
                    _Price = new GenericRepository<Price>(_dbContext);
                }
                return _Price;
            }
        }
        public IGenericRepository<Rental> Rental
        {
            get
            {
                if (_Rental == null)
                {
                    _Rental = new GenericRepository<Rental>(_dbContext);
                }
                return _Rental;
            }
        }
        public IGenericRepository<RentalAmenity> RentalAmenity
        {
            get
            {
                if (_RentalAmenity == null)
                {
                    _RentalAmenity = new GenericRepository<RentalAmenity>(_dbContext);
                }
                return _RentalAmenity;
            }
        }
        public IGenericRepository<Reservation> Reservation
        {
            get
            {
                if (_Reservation == null)
                {
                    _Reservation = new GenericRepository<Reservation>(_dbContext);
                }
                return _Reservation;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        //additional method added for garbage disposal

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}