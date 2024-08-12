using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        public int RentalId { get; set; }

        public int Daycount { get; set; }

        public int Percentage { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }

    }
}