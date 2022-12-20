using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClientManagement.Models
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var cs = "Server=(localdb)\\mssqllocaldb;Database=ClientManagement2;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(cs);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
