using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Core.Handlers
{
    public class HotelOffersRandomErrorHandler : AbstractHotelOfferStreamHandler
    {
        private readonly Random _random;

        public HotelOffersRandomErrorHandler()
        {
            _random = new Random();
        }

        public async override IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
            GetHotelOffersRequest request,
            IAsyncEnumerable<Result<HotelOffer>> inputStream,
            [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            await foreach (var response in inputStream)
            {
                var value = _random.Next(0, 9);

                if (value <= 2) 
                {
                    yield return new Exception("Oops, there was an error!");
                }
                else
                {
                    yield return response;
                }   
            }
        }
    }
}
