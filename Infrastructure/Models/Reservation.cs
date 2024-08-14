using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int RentalId { get; set; }

        [Required]
        public DateOnly Start { get; set; }

        [Required]
        public DateOnly End { get; set; }

        [Required]
        public bool Confirm { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }
    }
}
