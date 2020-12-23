using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public int YearOfPublication { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
