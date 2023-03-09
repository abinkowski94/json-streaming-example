using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JsonStreamingServer.Suppliers.Database.DbContexts;

public class HotelOffersDbContext : DbContext
{
    public HotelOffersDbContext(DbContextOptions<HotelOffersDbContext> options) : base(options)
    {
    }

    internal DbSet<HotelOfferEntity> HotelOffers { get; set; }
    internal DbSet<ImageEntity> Images { get; set; }
    internal DbSet<PricingEntity> Pricings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelOffersDbContext).Assembly);
        modelBuilder.SeedHotelOffers();
    }
}
