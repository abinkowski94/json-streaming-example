using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Abstractions.Handlers;

public interface IHotelOfferStreamHandler
{
    IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(GetHotelOffersRequest request, IAsyncEnumerable<Result<HotelOffer>> resultStream, CancellationToken cancellationToken);
}