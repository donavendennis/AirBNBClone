using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        public int RentalId { get; set; }

        // this is a datetime
        
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Amount { get; set; }

        public int Priority { get; set; }

        [ForeignKey("RentalId")]
        public Rental? Rental { get; set; }
    }
}
