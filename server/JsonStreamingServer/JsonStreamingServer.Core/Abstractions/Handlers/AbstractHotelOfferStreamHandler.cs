using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Abstractions.Handlers;

public abstract class AbstractHotelOfferStreamHandler : IHotelOfferStreamHandler
{
    private readonly IHotelOfferStreamHandler? _nextHandler;

    protected AbstractHotelOfferStreamHandler()
    {
    }

    protected AbstractHotelOfferStreamHandler(IHotelOfferStreamHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public abstract IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
        GetHotelOffersRequest request,
        IAsyncEnumerable<Result<HotelOffer>> inputStream,
        CancellationToken cancellationToken);

    IAsyncEnumerable<Result<HotelOffer>> IHotelOfferStreamHandler.GetHotelOffersAsync(
        GetHotelOffersRequest request,
        IAsyncEnumerable<Result<HotelOffer>> inputStream,
        CancellationToken cancellationToken)
    {
        var resultStream = TryGetHotelOffersAsync(request, inputStream, cancellationToken);

        if (_nextHandler is null)
        {
            return resultStream;
        }

        return _nextHandler.GetHotelOffersAsync(request, resultStream, cancellationToken);
    }

    private IAsyncEnumerable<Result<HotelOffer>> TryGetHotelOffersAsync(GetHotelOffersRequest request, IAsyncEnumerable<Result<HotelOffer>> inputStream, CancellationToken cancellationToken)
    {
        try
        {
            return GetHotelOffersAsync(request, inputStream, cancellationToken);
        }
        catch (Exception ex)
        {
            return GetHotelOffersAsyncFormError(ex);
        }
    }

    private static async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsyncFormError(Exception ex)
    {
        yield return await Task.FromResult<Result<HotelOffer>>(ex);
    }
}
