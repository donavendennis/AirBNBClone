using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    [PrimaryKey(nameof(FeeId), nameof(RentalId))]
    public class FeeRental
    {
        public int FeeId { get; set; }

        public int RentalId { get; set; }

        public int Amount { get; set; }

        [ForeignKey("FeeId")]
        public Fee? Fee { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}