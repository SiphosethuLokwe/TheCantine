using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CantinaAPI.Infrastructure.Data;

namespace Cantina.Infrastructure.Factories
{
    public class CantinaContextFactory : IDesignTimeDbContextFactory<CantinaContext>
    {
        public CantinaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CantinaContext>();
            optionsBuilder.UseSqlite("Data Source=DB/Cantina.db"); // also workaround for migration issue create instance of context at design time 

            return new CantinaContext(optionsBuilder.Options);
        }
    }
}
