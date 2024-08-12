using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public int RentalId { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}