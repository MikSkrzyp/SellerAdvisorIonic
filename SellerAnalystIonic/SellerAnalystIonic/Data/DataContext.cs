using Microsoft.EntityFrameworkCore;
using SellerAnalystIonic.Models;
using System.Collections.Generic;

namespace SellerAnalystIonic.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
