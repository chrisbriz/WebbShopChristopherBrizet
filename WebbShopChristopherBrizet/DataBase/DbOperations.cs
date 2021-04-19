using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebbShopChristopherBrizet.Models;

namespace WebbShopChristopherBrizet.DataBase
{
    public class DbOperations
    {
        private static WebShopContext context = new WebShopContext();

        /// <summary>
        /// Get user with id
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <returns>User object</returns>
        public static User GetUserById(int userId)
        {
            try
            {
                var user = new User();
                user = context.Users.FirstOrDefault(u => u.Id == userId);
                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int Login(string userName, string password)
        {

            try
            {

                var user = context.Users.FirstOrDefault(u => u.Name == userName && u.Password == password);

                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    user.IsActive = true;
                    user.SessionTimer = DateTime.Now;
                    context.SaveChanges();
                }
                return user.Id;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return 0;
            }
        }

        public static void Logout(int userId)
        {
            var user = GetUserById(userId);
            try
            {

                user.IsActive = false;
                user.SessionTimer = null;
                context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static bool BuyBook(int userId, int bookId)
        {
            var soldBook = new SoldBook();
            var user = GetUserById(userId);
            var book = GetBookById(bookId);
            var isLoggedIn = Ping(userId);

            if (isLoggedIn && user.IsActive)
            {
                if (book.Amount > 0)
                {                                       
                    soldBook.Title = book.Title;
                    soldBook.Author = book.Author;
                    soldBook.CategoryId = book.CategoryId;
                    soldBook.Price = book.Price;
                    soldBook.PurchasedDate = DateTime.Now;
                    soldBook.UserId = user.Id;                    
                    context.Add(soldBook);
                    book.Amount -= 1;
                    user.SessionTimer = DateTime.Now;
                    context.SaveChanges();
                    return true;

                }
                return false;
            }
            return false;
        }

        public static bool Register(string userName, string password, string passwordVerify)
        {
            var user = new User();

            if (password != passwordVerify)
            {
                return false;
            }

            user.Name = userName;
            user.Password = password;
            user.LastLogin = DateTime.Now;
            user.IsActive = true;
            user.IsAdmin = false;
            user.SessionTimer = DateTime.Now;
            context.Add(user);
            context.SaveChanges();
            return true;

        }

        public static bool Ping(int userId)
        {
            var user = GetUserById(userId);            

            if (user.SessionTimer?.AddMinutes(15) < DateTime.Now)
            {
                Logout(userId);
                return false;
            }
            else
            {
                Console.WriteLine("Pong");
                return true;
            }
        }

        public static List<Book> GetAllBooks()
        {
            try
            {
                var books = context.Books.ToList();
                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Book>();
            }
        }

        public static Book GetBookById(int bookId)
        {
            try
            {
                var book = GetAllBooks().FirstOrDefault(b => b.Id == bookId);
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Book();
            }
        }

        public static List<Book> GetBooksByKeyword(string keyword)
        {
            try
            {
                var category = GetAllCategories().FirstOrDefault(c => c.Name == keyword);
                if (category == null)
                {
                    category = new BookCategory();
                    category.Id = 0;
                }
                var books = GetAllBooks().Where(b => b.Title.Contains(keyword) || b.Author.Contains(keyword) || b.CategoryId == category.Id).ToList();

                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Book>();
            }
        }


        public static List<Book> GetBooksByCategory(int categoryId)
        {
            try
            {

                var books = GetAllBooks().Where(b => b.CategoryId == categoryId).ToList();
                return books;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Book>();
            }
        }

        public static List<Book> GetAvailableBooks(int categoryId)
        {
            try
            {
                var category = GetAllCategories().FirstOrDefault(c => c.Id == categoryId);
                var books = GetAllBooks().Where(b => b.CategoryId == category.Id && b.Amount > 0).ToList();
                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Book>();
            }
        }

        public static List<BookCategory> GetAllCategories()
        {
            var categories = new List<BookCategory>();
            try
            {

                categories = context.BookCategories.ToList();
                return categories;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<BookCategory>();
            }
        }

        public static BookCategory GetCategoriyById(int categoryId)
        {
            try
            {

                var category = GetAllCategories().FirstOrDefault(c => c.Id == categoryId);
                return category;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BookCategory();
            }
        }

        public static List<BookCategory> GetCategoriesByKeyword(string keyword)
        {
            try
            {
                var categories = GetAllCategories().Where(c => c.Name == keyword).ToList();
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<BookCategory>();
            }

        }
    }
}

