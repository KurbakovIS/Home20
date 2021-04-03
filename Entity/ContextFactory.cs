using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=HM21;Trusted_Connection=True;MultipleActiveResultSets=true";
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(ConnectionString,
                b => b.MigrationsAssembly("Entity"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
