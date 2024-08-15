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

        /*[ForeignKey("RentalId")]
        public virtual Rental? Rental { get; set; }*/

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }


        [Required]
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public double? OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        //For 3rd Party Stripe Credit Card Processing

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
