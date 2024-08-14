using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
