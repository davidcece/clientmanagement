using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ClientManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new ClientConfiguration(modelBuilder.Entity<Client>());
        }

    }

    //Configurations
    public class ClientConfiguration
    {
        public ClientConfiguration(EntityTypeBuilder<Client> entityBuilder)
        {
            
            entityBuilder.HasIndex(c => c.Email).IsUnique();
            entityBuilder.HasIndex(c => new { c.Email, c.InternationalPhone }).IsUnique();
            entityBuilder.Property(c => c.Email).HasMaxLength(100);
            entityBuilder.Property(c => c.Cellphone).HasMaxLength(12);
            entityBuilder.Property(c => c.InternationalPhone).HasMaxLength(12);
            entityBuilder.Property(c => c.EmailStatus).HasMaxLength(10);
            entityBuilder.Property(c => c.SmsStatus).HasMaxLength(10);
            entityBuilder.ToTable(c => c.HasCheckConstraint("CheckEmailStatus", "[EmailStatus] IN('Active','Removed')"));
            entityBuilder.ToTable(c => c.HasCheckConstraint("CheckSmsStatus", "[SmsStatus] IN('Active','Removed')"));
        }
    }

}
