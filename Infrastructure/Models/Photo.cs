using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int RentalId { get; set; }


        [Required]
        public byte[]? ImageData { get; set; }


        [Required]
        public string ImageType { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}