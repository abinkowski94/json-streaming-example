using Bogus;
using Bogus.Distributions.Gaussian;
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
        var faker = new Faker();

        for (int i = 0; i < 100; i++)
        {
            if (i % 10 == 0)
            { 
                await Task.Delay(1000, cancellationToken);
            }

            yield return await Task.FromResult(new HotelOffer
            {
                Id = Guid.NewGuid(),
                Name = $"{faker.Address.City()} hotel",
                Avaliability = new DateRange
                {
                    From = new DateOnly(2023, 10, 10),
                    To = new DateOnly(2023, 10, 25),
                },
                TotalPrice = new Price
                {
                    Value = faker.Random.GaussianDecimal(250, 50),
                    Currency = "EUR",
                },
            });
        }
    }
}
