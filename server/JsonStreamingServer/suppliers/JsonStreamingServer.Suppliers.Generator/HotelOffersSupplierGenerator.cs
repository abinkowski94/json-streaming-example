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
                var delay = faker.Random.GaussianInt(1000, 300);
                await Task.Delay(delay, cancellationToken);
            }

            var currency = faker.Finance.Currency();

            var hasShortDescription = faker.Random.Bool(0.8f);
            var hasDescription = hasShortDescription && faker.Random.Bool();

            var startDate = faker.Date.FutureDateOnly();
            var endDate = startDate.AddDays(faker.Random.GaussianInt(4, 3));

            yield return await Task.FromResult(new HotelOffer
            {
                Id = Guid.NewGuid(),
                Supplier = nameof(Generator),
                Name = $"{faker.Address.City()} hotel",
                Content = new Content
                {
                    ShortDescription = hasShortDescription ? faker.Lorem.Sentence() : null,
                    Description = hasDescription ? faker.Lorem.Paragraph() : null,
                },
                Avaliability = new DateRange
                {
                    From = startDate,
                    To = endDate,
                },
                TotalPrice = new Price
                {
                    Value = Math.Round(faker.Random.GaussianDecimal(250, 50), 2),
                    Currency = currency.Code,
                },
            });
        }
    }
}
