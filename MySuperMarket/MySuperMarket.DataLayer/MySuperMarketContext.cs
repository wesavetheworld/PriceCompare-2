using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySuperMarket.ClassLibrary;

namespace MySuperMarket.DataLayer
{
    public class MySuperMarketContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
