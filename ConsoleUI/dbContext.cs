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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transact> Transacts { get; set; }
        public DbSet<TransactDetail> TransactDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneralAccount>();
            modelBuilder.Entity<CreditAccount>();
            modelBuilder.Entity<TradingAccount>();
            modelBuilder.Entity<GeneralTransactDetail>();
            modelBuilder.Entity<TransferTransactDetail>();
            modelBuilder.Entity<ForexTransactDetail>();
            modelBuilder.Entity<TradingTransactDetail>();
        }
    }
    public class dbContextFactory : IDesignTimeDbContextFactory<dbContext>
    {
        public dbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<dbContext>();
            //optionsBuilder.UseSqlite("Data Source=C:/myApps/data/TestLab.db");
            var URLis = Environment.GetEnvironmentVariable("OneDriveConsumer").Replace("\\", "/") + "/AppData/anyData/TestLab.db";
            optionsBuilder.UseSqlite("Data Source=" + Environment.GetEnvironmentVariable("OneDriveConsumer").Replace("\\", "/") + "/AppData/anyData/TestLab.db");
            return new dbContext(optionsBuilder.Options);
        }
    }
}
