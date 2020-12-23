using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class BookAuthor
    {
        // Detta är en kopplingstabell för book och authors.
        // BookId och AuthorId är därför satta som primary key och foreign key.
        // detta görs i modelbuildern i Data/context
        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public Book Book { get; set; }

        public Author Author { get; set; }
    }
}
