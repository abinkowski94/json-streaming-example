using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Suppliers.Generator;

public class HotelOffersSupplierGenerator : IHotelOffersSupplier
{
    public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(
        GetHotelOffersRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return await Task.FromResult(new HotelOffer
        {
            Id = Guid.NewGuid(),
            Name = "Casey Key Resort - Gulf Shores",
            Avaliability = new DateRange
            {
                From = new DateOnly(2023, 10, 10),
                To = new DateOnly(2023, 10, 25),
            },
            TotalPrice = new Price
            {
                Value = 152m,
                Currency = "EUR",
            },
        });

        yield return await Task.FromResult(new HotelOffer
        {
            Id = Guid.NewGuid(),
            Name = "Addy's Villas",
            Avaliability = new DateRange
            {
                From = new DateOnly(2023, 10, 10),
                To = new DateOnly(2023, 10, 20),
            },
            TotalPrice = new Price
            {
                Value = 200,
                Currency = "EUR",
            },
        });
    }
}
