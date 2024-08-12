using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class FeeRental
    {
        [Key]
        public int FeeId { get; set; }

        [Key]
        public int RentalId { get; set; }

        public int Amount { get; set; }

        [ForeignKey("FeeId")]
        public Fee? Fee { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}