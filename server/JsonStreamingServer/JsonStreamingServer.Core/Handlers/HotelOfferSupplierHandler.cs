using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;

namespace JsonStreamingServer.Core.Handlers;

internal class HotelOfferSupplierHandler
{
    private readonly IReadOnlyList<IHotelOffersSupplier> _suppliers;

    public HotelOfferSupplierHandler(IReadOnlyList<IHotelOffersSupplier> suppliers)
    {
        _suppliers = suppliers;
    }

    public IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(GetHotelOffersRequest request, CancellationToken cancellationToken)
    {
        if (request.MixSupplierOffers)
        {
            return GetHotelOffersSupplierMixed(request, cancellationToken);
        }

        return GetHotelOffersSupplierBySupplier(request, cancellationToken);
    }

    private async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersSupplierMixed(GetHotelOffersRequest request, CancellationToken cancellationToken)
    {
        var streamEnumerators = _suppliers
            .Select(s => s.GetHotelOffers(request, cancellationToken).GetAsyncEnumerator(cancellationToken));

        foreach (var streamEnumerator in streamEnumerators)
        {
            if (await streamEnumerator.MoveNextAsync())
            {
                yield return streamEnumerator.Current;
            }
        }
    }

    private async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersSupplierBySupplier(GetHotelOffersRequest request, CancellationToken cancellationToken)
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
