using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Suppliers.Generator;

public class HotelOffersSupplier : IHotelOffersSupplier
{
    public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(GetHotelOffersRequest request, CancellationToken cancellationToken)
    {
        yield return new HotelOffer
        {
            Id = Guid.NewGuid(),
            Avaliability = new DateRange
            {
                From = new DateOnly(2023, 10, 10),
                To = new DateOnly(2023, 10, 25),
            },
            TotalPrice = new Price 
            { 
                Value = 0, 
                Currency = "EUR",
            },
        };
    }
}
