using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPE.DomainObjects.Approval;
using PPE.DomainObjects.PPE;

namespace PPE.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<cor_approvaldetail> cor_approvaldetail { get; set; }
        public DbSet<ppe_assetclassification> ppe_assetclassification { get; set; }
        public DbSet<ppe_additionform> ppe_additionform { get; set; }
        public DbSet<ppe_reassessment> ppe_reassessment { get; set; }
        public DbSet<ppe_register> ppe_register { get; set; }
        public DbSet<ppe_disposal> ppe_disposal { get; set; }
        public DbSet<ppe_dailyschedule> ppe_dailyschedule { get; set; }
        public DbSet<ppe_derecognition> ppe_derecognition { get; set; }
        public DbSet<ppe_lpo> ppe_lpo { get; set; }

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
