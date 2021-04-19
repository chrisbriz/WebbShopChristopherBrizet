using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebbShopChristopherBrizet.Models;

namespace WebbShopChristopherBrizet.DataBase
{
    public class WebShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<SoldBook> SoldBooks { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(@"Server=localhost;Database=WebShop;Trusted_Connection=True;");
        }

        /// <summary>
        /// Seeds the database if data does not exists
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User {Id = 1, Name = "Administrator", Password = "CodicRulez", IsAdmin = true },
                new User {Id = 2, Name = "TestCustomer", Password = "Codic2021", IsAdmin = false }
            );

            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory {Id = 1, Name = "Horror" },
                new BookCategory {Id = 2, Name = "Humor" },
                new BookCategory {Id = 3, Name = "Science Fiction" },
                new BookCategory {Id = 4, Name = "Romance" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book {Id = 1, Title = "Cabal (NightBreed)", Author = "Clive Barker", Price = 250, Amount = 3, CategoryId = 1 },    
                new Book {Id = 2, Title = "The Shinning", Author = "Stephen King", Price = 200, Amount = 2, CategoryId = 1 },    
                new Book {Id = 3, Title = "Doctor Sleep", Author = "Stephen King", Price = 200, Amount = 1, CategoryId = 1 },    
                new Book {Id = 4, Title = "The Shinning Robot", Author = "Isaac Asimov", Price = 150, Amount = 4, CategoryId = 3 }    
            );
        }
    }
}
