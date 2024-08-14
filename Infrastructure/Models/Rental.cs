using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}