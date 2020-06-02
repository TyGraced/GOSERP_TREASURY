using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GODP.APIsContinuation.DomainObjects.Supplier;

namespace Puchase_and_payables.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<cor_supplier> cor_supplier { get; set; }
        public DbSet<cor_supplierauthorization> cor_supplierauthorization { get; set; }
        public DbSet<cor_supplierbusinessowner> cor_supplierbusinessowner { get; set; }
        public DbSet<cor_supplierdocument> cor_supplierdocument { get; set; }
        public DbSet<cor_suppliertype> cor_suppliertype { get; set; }
        public DbSet<cor_topclient> cor_topclient { get; set; }
        public DbSet<cor_topsupplier> cor_topsupplier { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
