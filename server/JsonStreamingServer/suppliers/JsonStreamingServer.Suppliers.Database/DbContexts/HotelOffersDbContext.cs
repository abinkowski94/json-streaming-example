using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JsonStreamingServer.Suppliers.Database.DbContexts;

public class HotelOffersDbContext : DbContext
{
    public HotelOffersDbContext(DbContextOptions<HotelOffersDbContext> options) : base(options)
    {
    }

    internal DbSet<HotelOfferEntity> HotelOffers { get; init; }
    internal DbSet<ImageEntity> Images { get; init; }
    internal DbSet<PricingEntity> Pricings { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelOffersDbContext).Assembly);
        modelBuilder.SeedHotelOffers();
    }
}
