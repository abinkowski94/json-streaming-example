using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using JsonStreamingServer.Suppliers.FileStream.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Suppliers.FileStream
{
    public class HotelOffersFileStreamSupplier : IHotelOffersSupplier
    {
        private readonly ICsvHotelReaderService _csvHotelReaderService;

        public HotelOffersFileStreamSupplier(ICsvHotelReaderService csvHotelReaderService)
        {
            _csvHotelReaderService = csvHotelReaderService;
        }

        public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(
            GetHotelOffersRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await Task.Delay(1500, cancellationToken);

            yield return await Task.FromResult(_csvHotelReaderService.GetNext());
        }
    }
}