using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Core.Handlers
{
    public class HotelOffersMaxResultsHandler : AbstractHotelOfferStreamHandler
    {
        public async override IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
            GetHotelOffersRequest request,
            IAsyncEnumerable<Result<HotelOffer>> inputStream,
            [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            var count = 0;

            await foreach (var response in inputStream)
            {
                count++;

                if (!request.MaxResults.HasValue || count <= request.MaxResults.Value)
                {
                    yield return response;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
