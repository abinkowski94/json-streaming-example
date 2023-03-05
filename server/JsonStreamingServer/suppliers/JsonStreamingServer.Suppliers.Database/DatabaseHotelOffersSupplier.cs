using Bogus;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using JsonStreamingServer.Suppliers.Database.DbContexts;
using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Suppliers.Database;

public class DatabaseHotelOffersSupplier : IHotelOffersSupplier
{
    private static readonly Faker _faker = new();
    private readonly HotelOffersDbContext _context;

    public DatabaseHotelOffersSupplier(HotelOffersDbContext context)
    {
        _context = context;
    }

    public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(
        GetHotelOffersRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var hotelOfferEntities = _context.HotelOffers
            .Include(o => o.Pricings)
            .Include(o => o.Images)
            .AsAsyncEnumerable();

        await foreach(var hotelOfferEntity in hotelOfferEntities)
        {
            yield return new Result<HotelOffer>(Map(hotelOfferEntity));
        }
    }

    private static HotelOffer Map(HotelOfferEntity hotelOfferEntity) => new()
    {
        Id = Guid.Parse($"00000000-0000-0000-0000-{hotelOfferEntity.Id:000000000000}"),
        Supplier = nameof(Database),
        Avaliability = new DateRange
        {
            From = hotelOfferEntity.Pricings.Select(p => p.DayFrom).Min(),
            To = hotelOfferEntity.Pricings.Select(p => p.DayTo).Max(),
        },
        TotalPrice = new Price
        {
            Value = hotelOfferEntity.Pricings.Select(p => p.Value).Sum(),
            Currency = hotelOfferEntity.Pricings.First().Currency,
        },
        Name = hotelOfferEntity.Name,
        Content = new Content
        {
            Description = hotelOfferEntity.Description,
            ShortDescription = hotelOfferEntity.ShortDescription,
            Images = hotelOfferEntity.Images.Select(i => new Image
            {
                Url = i.Url,
                Caption = i.Caption,
            }).ToList(),
        },
        PriceBreakdown = hotelOfferEntity.Pricings.Select(p => new PriceBreakdownItem
        {
            Day = _faker.Date.BetweenDateOnly(p.DayFrom, p.DayTo),
            Price = new Price
            {
                Currency = p.Currency,
                Value = p.Value,
            },
            AgeRange = new AgeRange
            {
                From = p.AgeFrom,
                To = p.AgeTo,
            },
        }).ToList(),
    };
}
