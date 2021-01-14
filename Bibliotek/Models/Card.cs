using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Card
    {
        // Card är modellen för ett lånekort. Låntagarens data lagras därför i denna model

        public int CardId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public int TelephoneNumber { get; set; }
        
        // Räknar böcker som har ett försenat datum och inte har lämnats tillbaka 
        public int NotReturnedBooks
        {
            get
            {
                if (Rentals == null)
                    return 0;
                else
                    return Rentals.Where(r => r.RentedUntilDate.Date < DateTime.Now.Date && r.ReturnDate == null).Count();
            }
        }

        public ICollection<Rental> Rentals { get; set; }
    }
}
