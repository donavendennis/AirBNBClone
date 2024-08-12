using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class RentalAmenity
    {
        [Key]
        public int RentalId { get; set; }

        [Key]
        public int AmenityId { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

        [ForeignKey("AmenityId")]
        public Amenity? Amenity { get; set; }

    }
}