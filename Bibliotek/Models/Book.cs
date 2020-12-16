using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public int YearOfPublication { get; set; }

        public bool OnLoan { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
