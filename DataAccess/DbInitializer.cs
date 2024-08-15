using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility;


namespace DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public void Initialize()
        {
            _db.Database.EnsureCreated();

            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Amenities.Any() || _db.ApplicationUsers.Any() || _db.Discounts.Any() || _db.Fees.Any() || _db.FeeRentals.Any() || _db.Photos.Any() || _db.Prices.Any() || _db.Rental.Any() || _db.RentalAmenities.Any() || _db.Reservations.Any())
            {
                //DB has been seeded
            }
            else
            {
                var DateOnly_Today = DateOnly.FromDateTime(DateTime.Now);
                //create roles if they are not created
                //SD is a “Static Details” class we will create in Utility to hold constant strings for Roles

                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RenterRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.OwnerRole)).GetAwaiter().GetResult();

                //Create at least one "Super Admin" or “Admin”.  Repeat the process for other users you want to seed

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumber = "18015556919",
                    StreetAddress = "123 Main Street",
                    State = "UT",
                    PostalCode = "84408",
                    City = "Ogden",
                    EmailConfirmed = true,
                    Id = "deadbeef-6c0d-4d3e-8d1d-1d9444f119c4"
                }, "Admin123*").GetAwaiter().GetResult();

                // make a renter
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "renter@test.com",
                    Email = "renter@test.com",
                    FirstName = "Renter",
                    LastName = "Renter",
                    PhoneNumber = "18015556919",
                    StreetAddress = "123 Main Street",
                    State = "UT",
                    PostalCode = "84408",
                    City = "Ogden",
                    EmailConfirmed = true,
                    Id = "babebabe-6c0d-4d3e-8d1d-1d9444f119c5"
                }, "Renter123*").GetAwaiter().GetResult();

                // make a owner using similar testing email
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "owner@test.com",
                    Email = "owner@test.com",
                    FirstName = "Owner",
                    LastName = "Owner",
                    PhoneNumber = "18015556919",
                    StreetAddress = "123 Main Street",
                    State = "UT",
                    PostalCode = "84408",
                    City = "Ogden",
                    EmailConfirmed = true,
                    Id = "cafecafe-6c0d-4d3e-8d1d-1d9444f119c6"
                }, "Owner123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@test.com");

                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

                // now for renter

                user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "renter@test.com");
                _userManager.AddToRoleAsync(user, SD.RenterRole).GetAwaiter().GetResult();

                // now for owner

                user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "owner@test.com");
                _userManager.AddToRoleAsync(user, SD.OwnerRole).GetAwaiter().GetResult();

                var Amenities = new List<Amenity>
                {
                    new Amenity { Name = "Hot Tub", Description = "A hot tub is a great way to relax and have fun." },
                    new Amenity { Name = "Gym", Description = "A gym is a great way to relax and have fun." },
                    new Amenity { Name = "Sauna", Description = "A sauna is a great way to relax and have fun." }
                };

                foreach (var a in Amenities)
                {
                    _db.Amenities.Add(a);
                }

                _db.SaveChanges();


                // Rentals

                /*Id: An integer property representing a unique identifier.
    State: An integer property representing a state or region.
    Zip: An integer property representing a zip or postal code.
    City: An integer property representing a city.
    Address: An integer property representing an address.
    Phone: An integer property representing a phone number.
    OwnerId: A string property representing the owner's identifier.*/
                var Rentals = new List<Rental>
                {
                    new Rental { Title = "My Test House (posh)", Beds = 5, Baths = 5, Country = "PoshCountry", State = "State", Zip = "84408", City = "City", Address = "Addr", Phone = "8015556919", OwnerId = "deadbeef-6c0d-4d3e-8d1d-1d9444f119c4", Description = "MyDesc"},
                    new Rental { Title = "My Test House (cheap)", Beds = 1, Baths = 1, Country = "CheapCountry", State = "State", Zip = "84408", City = "City", Address = "Addr", Phone = "8015556919", OwnerId = "deadbeef-6c0d-4d3e-8d1d-1d9444f119c4", Description = "MyDesc2"},

                };

                foreach (var r in Rentals)
                {
                    _db.Rental.Add(r);
                }
                _db.SaveChanges();

                var RentalAmenities = new List<RentalAmenity>
                {
                    new RentalAmenity { RentalId = 1, AmenityId = 1 },
                    new RentalAmenity { RentalId = 1, AmenityId = 2 },
                    new RentalAmenity { RentalId = 1, AmenityId = 3 },
                    new RentalAmenity { RentalId = 2, AmenityId = 1 },
                };

                foreach (var ra in RentalAmenities)
                {
                    _db.RentalAmenities.Add(ra);
                }

                _db.SaveChanges();

                // Discounts
                /*
                 * Id: This is a public integer property that serves as the unique identifier for the object.
                RentalId: This is a public integer property that represents the ID of the associated rental.
                Daycount: This is a public integer property that likely represents the number of days associated with the object.
                Percentage: This is a public integer property that possibly represents a percentage value associated with the object.
                Rental: This is a public property that represents a reference to a Rental object. The [ForeignKey("RentalId")] attribute indicates that this property is a foreign key, meaning it is used to establish a relationship between this object and the Rental object.*/

                var Discounts = new List<Discount>
                {
                    new Discount { RentalId = 1, Daycount = 7, Percentage = 10 },
                    new Discount { RentalId = 1, Daycount = 14, Percentage = 20 },
                    new Discount { RentalId = 1, Daycount = 30, Percentage = 30 },
                    new Discount { RentalId = 2, Daycount = 365, Percentage = 1 }
                };

                foreach (var d in Discounts)
                {
                    _db.Discounts.Add(d);
                }

                _db.SaveChanges();

                // Fees

                /*Id, Name, Description*/

                var Fees = new List<Fee>
                {
                    new Fee { Name = "Cleaning Fee", Description = "A fee for cleaning the rental." },
                    new Fee { Name = "Service Fee", Description = "A fee for services provided." },
                    new Fee { Name = "Tax", Description = "A tax fee." }
                };

                foreach (var f in Fees)
                {
                    _db.Fees.Add(f);
                }
                _db.SaveChanges();

                // FeeRentals

                /*FeeId: An integer property representing the ID of the associated fee.
                 * RentalId: An integer property representing the ID of the associated rental.
                 */

                var FeeRentals = new List<FeeRental>
                {
                    new FeeRental { FeeId = 1, RentalId = 1, Amount = 1 },
                    new FeeRental { FeeId = 1, RentalId = 2, Amount = 10 },
                    new FeeRental { FeeId = 2, RentalId = 2, Amount = 10 },
                    new FeeRental { FeeId = 3, RentalId = 2, Amount = 10 }
                };

                foreach (var fr in FeeRentals)
                {
                    _db.FeeRentals.Add(fr);
                }
                _db.SaveChanges();

                // Price


                var Prices = new List<Price>
                {
                    new Price { RentalId = 1, Start = DateOnly_Today.AddDays(-10), End = DateOnly_Today.AddDays(100), Amount = 300, Priority = 1 },
                    new Price { RentalId = 1, Start = DateOnly_Today.AddDays(20), End = DateOnly_Today.AddDays(40), Amount = 250, Priority = 2 },
                    new Price { RentalId = 1, Start = DateOnly_Today.AddDays(25), End = DateOnly_Today.AddDays(35), Amount = 200, Priority = 3 },
                    new Price { RentalId = 2, Start = DateOnly_Today, End = DateOnly_Today.AddDays(100), Amount = 100, Priority = 1 }
                };

                foreach (var p in Prices)
                {
                    _db.Prices.Add(p);
                }
                _db.SaveChanges();

                // Photos

                /*Id: This is a public integer property that likely serves as the unique identifier for this entity.
                RentalId: This is a public integer property that likely represents a foreign key relationship to another entity, possibly a "Rental" entity.
                ImageData: This is a public byte array property that can be nullable (byte[]?). It likely represents the binary data of an image associated with this entity.
                ImageType: This is a public string property that likely represents the type or format of the image data (e.g., "image/jpeg", "image/png").
                Rental: This is a public property of type Rental that is marked with the [ForeignKey("RentalId")] attribute. This indicates that this property represents the navigation property for the foreign key relationship defined by the RentalId property, likely allowing access to the related Rental entity.*/


                byte[] bytes_emptyphoto = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQAAAAA3bvkkAAAACklEQVR4AWNgAAAAAgABc3UBGAAAAABJRU5ErkJggg==");

                // image file names are cheap_house.png, posh_house.png
                // a list of the above
                var imageFiles = new List<string>
                {
                    "cheap_house.png",
                    "posh_house.png"
                };

                // dictionary, key string, value bytes
                var imageBytes = new Dictionary<string, byte[]>();

                // for each image in the image file, read the file and convert to byte array
                foreach (var imageFile in imageFiles)
                {
                    // the images are stored in the wwwroot/images/preset folder
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/preset", imageFile);
                    bytes_emptyphoto = System.IO.File.ReadAllBytes(path);
                    // then put the bytes in a dictionary with the key being the image file name
                    imageBytes.Add(imageFile, bytes_emptyphoto);

                }

                var Photos = new List<Photo>
                {
                    new Photo { RentalId = 1, ImageData = bytes_emptyphoto, ImageType = "image/png", PrimaryImage = false},
                    new Photo { RentalId = 1, ImageData = bytes_emptyphoto, ImageType = "image/png", PrimaryImage = false },
                    new Photo { RentalId = 1, ImageData = bytes_emptyphoto, ImageType = "image/png", PrimaryImage = false },
                    new Photo { RentalId = 2, ImageData = bytes_emptyphoto, ImageType = "image/png", PrimaryImage = false },

                    // make the posh house with the posh image
                    new Photo { RentalId = 1, ImageData = imageBytes["posh_house.png"], ImageType = "image/png", PrimaryImage = true},
                    // now the cheap house with the cheap image
                    new Photo { RentalId = 2, ImageData = imageBytes["cheap_house.png"], ImageType = "image/png", PrimaryImage = true }
                };

                foreach (var p in Photos)
                {
                    _db.Photos.Add(p);
                }
                _db.SaveChanges();

                // reservations


                var Reservations = new List<Reservation>
                {
                    new Reservation { UserId = "babebabe-6c0d-4d3e-8d1d-1d9444f119c5", RentalId = 1, Start = DateOnly_Today.AddDays(5), End = DateOnly_Today.AddDays(10), Confirm = true, OrderDate = DateTime.Now },
                    new Reservation { UserId = "babebabe-6c0d-4d3e-8d1d-1d9444f119c5", RentalId = 1, Start = DateOnly_Today.AddDays(15), End = DateOnly_Today.AddDays(15), Confirm = false , OrderDate = DateTime.Now},
                };

                foreach (var r in Reservations)
                {
                    _db.Reservations.Add(r);
                }
                _db.SaveChanges();
            }

        }
    }
}
