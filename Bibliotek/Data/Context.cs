using Bibliotek.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Här sätter jag ut nycklarna till kopplingstabellen bookauthor

            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
              .HasOne(ba => ba.Author)
              .WithMany(ba => ba.BookAuthors)
              .HasForeignKey(ba => ba.AuthorId);

        }

        // Det här är referenser som används för att lagra data till de olika modellerna.

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<Inventory> Inventories { get; set; }
    }
}
