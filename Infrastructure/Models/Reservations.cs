using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Reservations
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RentalId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Confirm { get; set; }
    }
}
