using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using JsonStreamingServer.Suppliers.FileStream.Services;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Suppliers.FileStream
{
    public class HotelOffersFileStreamSupplier : IHotelOffersSupplier
    {
        private readonly ILogger<CsvHotelOfferReader> _logger;
        private readonly string _filePath;

        public HotelOffersFileStreamSupplier(ILogger<CsvHotelOfferReader> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "HotelOffers.csv");
        }

        public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(
            GetHotelOffersRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var reader = new CsvHotelOfferReader(_logger, _filePath);

            var hotelOffer = await reader.GetNextOfferAsync(cancellationToken);

            while (hotelOffer is not null)
            {
                yield return hotelOffer;

                hotelOffer = await reader.GetNextOfferAsync(cancellationToken);
            }

            yield break;
        }
    }
}