using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Extensions;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Handlers;

public class HotelOfferSupplierHandler : FirstHotelOfferStreamHandler
{
    private readonly IEnumerable<IHotelOffersSupplier> _suppliers;

    public HotelOfferSupplierHandler(
        IHotelOfferStreamHandler nextHandler,
        IEnumerable<IHotelOffersSupplier> suppliers)
        : base(nextHandler)
    {
        _suppliers = suppliers;
    }

    public override IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
        GetHotelOffersRequest request,
        CancellationToken cancellationToken)
    {
        var streams = _suppliers.Select(s => GetSupplierResults(s, request, cancellationToken));

        if (request.MixSupplierOffers)
        {
            return streams.ZipResultAsyncEnumerables(cancellationToken);
        }
        else
        {
            return streams.ConcatResultAsyncEnumerables(cancellationToken);
        }
    }

    private static IAsyncEnumerable<Result<HotelOffer>> GetSupplierResults(
        IHotelOffersSupplier supplier,
        GetHotelOffersRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return supplier.GetHotelOffers(request, cancellationToken);
        }
        catch (Exception ex)
        {
            return new Result<HotelOffer>(ex)
                .ToAsyncEnumerable();
        }
    }
}
