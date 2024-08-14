using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    [PrimaryKey(nameof(FeeId), nameof(RentalId))]
    public class FeeRental
    {

        [Required]
        public int FeeId { get; set; }

        [Required]
        public int RentalId { get; set; }

        [Required]
        public int Amount { get; set; }

        [ForeignKey("FeeId")]
        public Fee? Fee { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}