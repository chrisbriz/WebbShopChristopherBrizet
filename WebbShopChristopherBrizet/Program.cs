using System;
using WebbShopChristopherBrizet.DataBase;
using WebbShopChristopherBrizet.Models;

namespace WebbShopChristopherBrizet
{
    class Program
    {
        static void Main(string[] args)
        {
            //Exempel 1
            WebbShopAPI.Login("TestCustomer", "Codic2021");
            var categoryList = WebbShopAPI.GetAllCategories();
            var bookCategoryList = WebbShopAPI.GetBooksByCategory(1);
            var bookList = WebbShopAPI.GetAvailableBooks(1);
            foreach (var item in bookList)
            {
                Console.WriteLine($"{item.Title} - {item.Amount} available");
            }
            var book = WebbShopAPI.GetBookById(3);
            WebbShopAPI.BuyBook(1, 3);
            WebbShopAPI.Logout(1);


            //exempel 2
            WebbShopAPI.Login("Administrator", "CodicRulez");
            WebbShopAPI.AddCategory(1, "Action");
            var categories = WebbShopAPI.GetAllCategories();
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id}");
            }
            //WebbShopAPI.AddBookToCategory();

            //exempel 3
            WebbShopAPI.Login("Administrator", "CodicRulez");
            WebbShopAPI.AddUser(1, "thisDude", "passforlife");
        }
    }
}
