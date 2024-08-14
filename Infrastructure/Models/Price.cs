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

        // this is a datetime

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int Priority { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }
    }
}
