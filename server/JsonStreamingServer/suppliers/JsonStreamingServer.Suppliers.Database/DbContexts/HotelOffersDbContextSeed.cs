using Bogus;
using Bogus.Distributions.Gaussian;
using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JsonStreamingServer.Suppliers.Database.DbContexts;

public static class HotelOffersDbContextSeed
{
    private static readonly int _seed = 21_37;
    private static readonly Faker _faker = new() { Random = new Randomizer(_seed) };

    public static void SeedHotelOffers(this ModelBuilder modelBuilder)
    {
        var imageId = 1;
        var pricingId = 1;

        for (var id = 1; id <= 1000; id++)
        {
            var hasShortDescription = _faker.Random.Bool(0.8f);
            var hasDescription = hasShortDescription && _faker.Random.Bool();

            var offer = new HotelOfferEntity
            {
                Id = id,
                Name = $"{_faker.Address.City()} hotel",
                ShortDescription = hasShortDescription ? _faker.Lorem.Sentence() : null,
                Description = hasDescription ? _faker.Lorem.Paragraph() : null,
            };

            modelBuilder.Entity<HotelOfferEntity>()
                .HasData(offer);

            modelBuilder.SeedImages(ref imageId, offer.Id);
            modelBuilder.SeedPricings(ref pricingId, offer.Id);
        }
    }

    private static void SeedImages(this ModelBuilder modelBuilder, ref int imageId, int offerId)
    {
        var numberOfImages = _faker.Random.Int(0, 3);

        for (var i = 0; i < numberOfImages; i++)
        {
            modelBuilder.Entity<ImageEntity>()
                .HasData(new
                {
                    id = imageId++,
                    offer_id = offerId,
                    Url = _faker.Image.LoremFlickrUrl(),
                    Caption = _faker.Lorem.Word(),
                });
        }
    }

    private static void SeedPricings(this ModelBuilder modelBuilder, ref int pricingId, int offerId)
    {
        var numberOfPricings = _faker.Random.Int(1, 3);
        var currency = _faker.Finance.Currency();

        for (var i = 0; i < numberOfPricings; i++)
        {
            var startDate = _faker.Date.FutureDateOnly();
            var endDate = startDate.AddDays(_faker.Random.GaussianInt(4, 3));
            var (ageFrom, ageTo) = GetAgeRange(i, numberOfPricings);

            modelBuilder.Entity<PricingEntity>()
                .HasData(new
                {
                    id = pricingId++,
                    offer_id = offerId,
                    DayFrom = startDate,
                    DayTo = endDate,
                    AgeFrom = ageFrom,
                    AgeTo = ageTo,
                    Value = Math.Round(_faker.Random.GaussianDecimal(100, 50), 2),
                    Currency = currency.Code,
                });
        }
    }

    private static (uint ageFrom, uint ageTo) GetAgeRange(int i, int numberOfPricings) => numberOfPricings switch
    {
        2 => i switch
        {
            2 => (65, 200),
            1 => (18, 65),
            _ => (2, 18),
        },
        1 => i switch
        {
            1 => (18, 200),
            _ => (2, 18),
        },
        _ => (18, 65)
    };
}
