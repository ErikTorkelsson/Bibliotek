using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        
        public int BookId { get; set; }
        public Book Book { get; set; }

        public ICollection<Rental> Rentals { get; set; }

        public bool Available
        {
            // Detta är en read only efterson det inte har satts någon set.
            //Så vid varje anrop ska Available visas som false endast om checken nedanför går igenom. 
            get
            {
                if (Rentals == null)
                    return true;
                else if (Rentals.Count == 0)
                    return true;
                else if (Rentals.All(r => r.Returned))
                    return true;
                else
                {
                    return false;
                }
            }
        }
    }
}
