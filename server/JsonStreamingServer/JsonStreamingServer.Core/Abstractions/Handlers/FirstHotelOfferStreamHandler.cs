using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Abstractions.Handlers;

public abstract class FirstHotelOfferStreamHandler : AbstractHotelOfferStreamHandler, IHotelOfferRequestHandler
{
    protected FirstHotelOfferStreamHandler() 
        : base()
    {
    }

    protected FirstHotelOfferStreamHandler(IHotelOfferStreamHandler nextHandler)
        : base(nextHandler)
    {
    }

    public abstract IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
        GetHotelOffersRequest request,
        CancellationToken cancellationToken);

    public sealed override IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
        GetHotelOffersRequest request,
        IAsyncEnumerable<Result<HotelOffer>> inputStream,
        CancellationToken cancellationToken)
    {
        return GetHotelOffersAsync(request, cancellationToken);
    }

    IAsyncEnumerable<Result<HotelOffer>> IHotelOfferRequestHandler.GetHotelOffersAsync(
        GetHotelOffersRequest request,
        CancellationToken cancellationToken)
    {
        return (this as IHotelOfferStreamHandler).GetHotelOffersAsync(request, resultStream: null!, cancellationToken);
    }
}
