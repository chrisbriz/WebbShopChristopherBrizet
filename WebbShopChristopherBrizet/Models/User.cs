using System;
using System.Collections.Generic;
using System.Text;

namespace WebbShopChristopherBrizet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime? SessionTimer { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

    }
}
