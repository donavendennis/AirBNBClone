using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RentalId { get; set; }

        [Required]
        public int Daycount { get; set; }

        [Required]
        public int Percentage { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}