using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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