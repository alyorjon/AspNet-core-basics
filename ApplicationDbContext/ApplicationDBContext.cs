using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.ApplicationDbContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Book> Book{ get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Portfolio - Many to Many relationship
            builder.Entity<Portfolio>(x => 
            {
                x.HasKey(p => new { p.AppUserId, p.StockId }); // Composite key
            });

            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(u => u.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(u => u.StockId);

            // Stock - Comment relationship
            builder.Entity<Comment>()
                .HasOne(c => c.Stock)
                .WithMany(s => s.Comment)
                .HasForeignKey(c => c.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser - Comment relationship
            builder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comment)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

            var fixedDate = new DateTime(2025, 1, 1, 10, 0, 0); // Static date

            List<Stock> stocks = new List<Stock>
            {
                new Stock
                {
                    Id = 1,
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Price = 420.50m,
                    Industry = "Technology",
                    MarketCap = 3120000000000,
                    LastUpdated = fixedDate // Static value
                },
                new Stock
                {
                    Id = 2,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Price = 180.25m,
                    Industry = "Technology",
                    MarketCap = 2890000000000,
                    LastUpdated = fixedDate.AddHours(1) // Static value
                },
                new Stock
                {
                    Id = 3,
                    Symbol = "GOOGL",
                    CompanyName = "Alphabet Inc.",
                    Price = 135.75m,
                    Industry = "Technology",
                    MarketCap = 1720000000000,
                    LastUpdated = fixedDate.AddHours(2) // Static value
                }
            };

            builder.Entity<Stock>().HasData(stocks);
        }
    }
}