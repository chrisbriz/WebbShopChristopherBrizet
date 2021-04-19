using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WebbShopChristopherBrizet.Models;

namespace WebbShopChristopherBrizet.DataBase
{
    public class WebbShopAPI
    {
        private static WebShopContext context = new WebShopContext();

        #region Common operations

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
                return new User();
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName">string</param>
        /// <param name="password">string</param>
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

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="userId"></param>
        public static void Logout(int userId)
        {
            try
            {
                var user = GetUserById(userId);
                user.IsActive = false;
                user.SessionTimer = null;
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Buy the selected book
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        public static void BuyBook(int userId, int bookId)
        {
            var soldBook = new SoldBook();
            var user = GetUserById(userId);
            var book = GetBookById(bookId);
            var isLoggedIn = Ping(userId);
            try
            {
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
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Register yourself as customer
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="passwordVerify"></param>
        public static void Register(string userName, string password, string passwordVerify)
        {
            try
            {
                var user = new User();

                if (password != passwordVerify)
                {
                    return;
                }

                user.Name = userName;
                user.Password = password;
                user.LastLogin = DateTime.Now;
                user.IsActive = true;
                user.IsAdmin = false;
                user.SessionTimer = DateTime.Now;
                context.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true or false</returns>
        public static bool Ping(int userId)
        {
            var user = GetUserById(userId);

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Get a list of all books
        /// </summary>
        /// <returns>List<Book></returns>
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

        /// <summary>
        /// Get specific book with Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>Book object</returns>
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

        /// <summary>
        /// Get all books matching specific keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>list of books</returns>
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

        /// <summary>
        /// Get all books in a specific category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>list of books</returns>
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

        /// <summary>
        /// Show list of books with amout above 0
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>list of books</returns>
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

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>list of categories</returns>
        public static List<BookCategory> GetAllCategories()
        {
            try
            {
                var categories = new List<BookCategory>();
                categories = context.BookCategories.ToList();
                return categories;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<BookCategory>();
            }
        }

        /// <summary>
        /// get category with specific id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>book category</returns>
        public static BookCategory GetCategoryById(int categoryId)
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

        /// <summary>
        /// Get categories matching specified keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>lost of categories</returns>
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

        #endregion

        #region Admin Operations

        /// <summary>
        /// Add one book
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        public static void AddBook(int adminId, int bookId, string title, string author, int price, int amount)
        {

            if (IsAdmin(adminId))
            {
                try
                {
                    var book = GetBookById(bookId);
                    if (book != null)
                    {
                        book.Amount += amount;
                        return;
                    }
                    book.Title = title;
                    book.Author = author;
                    book.Price = price;
                    book.Amount += amount;
                    context.Add(book);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the amount of books available
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="amount"></param>
        public static void SetAmount(int adminId, int bookId, int amount)
        {

            if (IsAdmin(adminId))
            {
                try
                {
                    var book = GetBookById(bookId);
                    book.Amount = amount;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        /// <summary>
        /// Show all users
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>list of users</returns>
        public static List<User> GetAllUsers(int adminId)
        {
            if (IsAdmin(adminId))
            {
                var users = new List<User>();
                try
                {
                    users = context.Users.ToList();
                    return users;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return new List<User>();
        }

        /// <summary>
        /// Find users matching specified keyword
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="keyword"></param>
        /// <returns>list of users</returns>
        public static List<User> FindUser(int adminId, string keyword)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var users = GetAllUsers(adminId).Where(u => u.Name.Contains(keyword)).ToList();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return new List<User>();
        }

        /// <summary>
        /// Update entry for book
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="price"></param>
        public static void UpdateBook(int adminId, int bookId, string title, string author, int price)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var book = GetBookById(bookId);
                    book.Title = title;
                    book.Author = author;
                    book.Price = price;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Remove book from db
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        public static void DeleteBook(int adminId, int bookId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var book = GetBookById(bookId);
                    if (book.Amount <= 1)
                    {
                        context.Remove(book);
                        context.SaveChanges();
                        return;
                    }
                    book.Amount -= 1;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Add a category to the db
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="name"></param>
        public static void AddCategory(int adminId, string name)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var category = GetCategoriesByKeyword(name);

                    if (category == null)
                    {
                        var newCategory = new BookCategory();
                        newCategory.Name = name;
                        context.Add(newCategory);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Add a book to a category
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="categoryId"></param>
        public static void AddBookToCategory(int adminId, int bookId, int categoryId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var book = GetBookById(bookId);
                    book.CategoryId = categoryId;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the name of the category
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="categoryId"></param>
        /// <param name="name"></param>
        public static void UpdateCategory(int adminId, int categoryId, string name)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var category = GetCategoryById(categoryId);
                    category.Name = name;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Remove category from db if it is empty
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="categoryId"></param>
        public static void DeleteCategory(int adminId, int categoryId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var category = GetCategoryById(categoryId);
                    var books = GetBooksByCategory(categoryId);
                    if (books == null)
                    {
                        context.Remove(category);
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void AddUser(int adminId, string userName, string password)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var users = FindUser(adminId, userName);
                    if (users == null && !string.IsNullOrEmpty(password))
                    {
                        var user = new User();
                        user.Name = userName;
                        user.Password = password;
                        user.LastLogin = DateTime.Now;
                        user.IsActive = false;
                        user.IsAdmin = false;
                        user.SessionTimer = null;
                        context.Add(user);
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// List all books sold
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>List of SoldBooks</returns>
        public static List<SoldBook> GetSoldBooks(int adminId)
        {
            var soldBooks = new List<SoldBook>();
            try
            {
                if (IsAdmin(adminId))
                {

                    soldBooks = context.SoldBooks.ToList();
                    return soldBooks;
                }
                return soldBooks;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Show total money earned
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static int MoneyEarned(int adminId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var soldBooks = GetSoldBooks(adminId);
                    var totalMoney = soldBooks.Sum(b => b.Price);
                    return totalMoney;
                }
                return 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Show the user who bought most books
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>User</returns>
        public static User BestCustomer(int adminId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var soldBooks = GetSoldBooks(adminId);
                    var bestCustomer = soldBooks.GroupBy(s => s.UserId).Select(item => new { Id = item.Key, Count = item.Count() }).OrderByDescending(item => item.Count).First();
                    var user = new User();
                    user = GetUserById(bestCustomer.Id);
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return new User();
        }

        /// <summary>
        /// Set user to admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        public static void Promote(int adminId, int userId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var user = GetUserById(userId);
                    user.IsAdmin = true;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// remove admin flag from user
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        public static void Demote(int adminId, int userId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var user = GetUserById(userId);
                    user.IsAdmin = false;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// set active status
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        public static void ActivateUser(int adminId, int userId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var user = GetUserById(userId);
                    user.IsActive = true;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// remove active status
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        public static void InactivateUser(int adminId, int userId)
        {
            try
            {
                if (IsAdmin(adminId))
                {
                    var user = GetUserById(userId);
                    user.IsActive = false;
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        private static bool IsAdmin(int adminId)
        {
            var user = GetUserById(adminId);
            if (user.IsAdmin)
            {
                return true;
            }
            return false;
        }
    }
}

