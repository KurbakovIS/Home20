using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class DataContext : DbContext
    {
        public DbSet<Food> Foods {get;set;}

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
