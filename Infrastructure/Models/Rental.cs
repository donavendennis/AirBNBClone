using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        public int State { get; set; }

        public int Zip { get; set; }

        public int City { get; set; }

        public int Address { get; set; }

        public int Phone { get; set; }

        public int OwnerId { get; set; }

    }
}