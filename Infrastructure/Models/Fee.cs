using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}