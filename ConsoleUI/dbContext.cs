using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options) { }
        public DbSet<MasterKey> MasterKeys { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transact> Transacts { get; set; }
        public DbSet<TransactDetail> TransactDetails { get; set; }
        public DbSet<TransactTag> TransactTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneralAccount>();
            modelBuilder.Entity<CreditAccount>();
            modelBuilder.Entity<TradingAccount>();
            modelBuilder.Entity<GeneralTransactDetail>()
                .Property(c => c.CategoryId)
                .HasColumnName(nameof(GeneralTransactDetail.CategoryId));
            modelBuilder.Entity<GeneralTransactDetail>()
                .Property(s => s.SubcategoryId)
                .HasColumnName(nameof(GeneralTransactDetail.SubcategoryId));
            modelBuilder.Entity<CreditTransactDetail>();
            modelBuilder.Entity<TransferTransactDetail>();
            modelBuilder.Entity<ForexTransactDetail>();
            modelBuilder.Entity<TradingFromTransactDetail>()
                .Property(c => c.CategoryId)
                .HasColumnName(nameof(TradingFromTransactDetail.CategoryId));
            modelBuilder.Entity<TradingFromTransactDetail>()
                .Property(s => s.SubcategoryId)
                .HasColumnName(nameof(TradingFromTransactDetail.SubcategoryId));
            modelBuilder.Entity<TradingTransactDetail>();
        }
    }
    public class dbContextFactory : IDesignTimeDbContextFactory<dbContext>
    {
        public dbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<dbContext>();
            var URLis = Environment.GetEnvironmentVariable("OneDriveConsumer").Replace("\\", "/") + "/AppData/anyData/TestLab.db";
            optionsBuilder.UseSqlite("Data Source=" + URLis);
            return new dbContext(optionsBuilder.Options);
        }
    }
}
