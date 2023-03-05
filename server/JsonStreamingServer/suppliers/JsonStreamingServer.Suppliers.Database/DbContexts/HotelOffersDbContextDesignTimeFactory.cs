using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JsonStreamingServer.Suppliers.Database.DbContexts;

public class HotelOffersDbContextDesignTimeFactory : IDesignTimeDbContextFactory<HotelOffersDbContext>
{
    public HotelOffersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelOffersDbContext>();
        optionsBuilder.UseSqlite("Data Source=DbFiles/hotels.db");

        return new HotelOffersDbContext(optionsBuilder.Options);
    }
}
