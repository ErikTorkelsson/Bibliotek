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
            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
              .HasOne(ba => ba.Author)
              .WithMany(ba => ba.BookAuthors)
              .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<Rental>()
            .Property(l => l.RentalDate)
            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Rental>()
            .Property(l => l.ReturnDate)
            .HasDefaultValueSql("DATEADD(MONTH, 1, GETDATE())");
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Rental> Loans { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }
    }
}
