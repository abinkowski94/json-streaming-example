using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Services;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Services;

public class HotelService : IHotelService
{
    private readonly IHotelOfferRequestHandler _requestHandler;

    public HotelService(IHotelOfferRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }

    public IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(GetHotelOffersRequest request, CancellationToken cancellationToken)
    {
        return _requestHandler.GetHotelOffersAsync(request, cancellationToken);
    }
}
