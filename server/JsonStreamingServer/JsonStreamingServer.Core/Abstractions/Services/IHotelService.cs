using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Abstractions.Services;

public interface IHotelService
{
    IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(GetHotelOffersRequest request, CancellationToken cancellationToken);
}