using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    [PrimaryKey(nameof(RentalId), nameof(AmenityId))]
    public class RentalAmenity
    {
        public int RentalId { get; set; }

        public int AmenityId { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

        [ForeignKey("AmenityId")]
        public Amenity? Amenity { get; set; }

    }
}