using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public int Name { get; set; }

        public int Description { get; set; }

    }
}