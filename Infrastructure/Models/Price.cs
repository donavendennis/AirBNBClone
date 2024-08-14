using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RentalId { get; set; }

        // this is a dateonly

        [Required]
        public DateOnly Start { get; set; }

        [Required]
        public DateOnly End { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int Priority { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }
    }
}
