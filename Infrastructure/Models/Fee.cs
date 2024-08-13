using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}