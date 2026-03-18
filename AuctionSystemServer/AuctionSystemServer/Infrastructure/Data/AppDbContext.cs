using Microsoft.EntityFrameworkCore;
using AuctionSystemServer.Core.Entities;

namespace AuctionSystem.API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Auction> Auctions => Set<Auction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auction>()
                .Property(a => a.CurrentHighBid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Auction>()
                .Property(a => a.RowVersion)
                .IsRowVersion();
        }
    }
}