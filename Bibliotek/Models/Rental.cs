using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Rental
    {
        public int RentalId { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime RentedUntilDate { get; set; }

        public DateTime? ReturnDate { get; set; }
 
        public bool Returned { get { return ReturnDate != null; } }
        

    }
}
