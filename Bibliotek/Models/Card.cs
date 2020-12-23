using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Card
    {
        public int CardId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
