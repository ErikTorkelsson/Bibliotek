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

        public string Email { get; set; }

        public int TelephoneNumber { get; set; }

        public int NotReturnedBooks
        {
            get
            {
                if (Rentals == null)
                    return 0;
                else
                    return Rentals.Where(r => r.RentedUntilDate < DateTime.Now && r.ReturnDate == null).Count();
            }
        }

        public ICollection<Rental> Rentals { get; set; }
    }
}
