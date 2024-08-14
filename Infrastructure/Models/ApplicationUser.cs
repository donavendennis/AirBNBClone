using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required]
        [DisplayName("Street Address")]
        public string? StreetAddress { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
