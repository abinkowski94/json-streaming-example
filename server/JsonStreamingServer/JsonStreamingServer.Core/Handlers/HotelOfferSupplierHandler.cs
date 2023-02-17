using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

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
        if (request.MixSupplierOffers)
        {
            return GetHotelOffersSupplierMixed(request, cancellationToken);
        }

        return GetHotelOffersSupplierBySupplier(request, cancellationToken);
    }

    private async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersSupplierMixed(
        GetHotelOffersRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var streamEnumerators = _suppliers
            .Select(s => s.GetHotelOffers(request, cancellationToken).GetAsyncEnumerator(cancellationToken));

        try
        {
            foreach (var streamEnumerator in streamEnumerators)
            {
                if (await streamEnumerator.MoveNextAsync())
                {
                    yield return streamEnumerator.Current;
                }
            }
        }
        finally
        {
            foreach (var streamEnumerator in streamEnumerators)
            {
                await streamEnumerator.DisposeAsync();
            }
        }        
    }

    private async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersSupplierBySupplier(
        GetHotelOffersRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var supplier in _suppliers)
        {
            await foreach (var supplierResult in supplier.GetHotelOffers(request, cancellationToken))
            {
                yield return supplierResult;
            }
        }
    }
}
