using Home20.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Home20.Entity
{
    public class DataContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;DataBase=_EntityCoreHome;Trusted_Connection=True;"
            );
        }
    }
}