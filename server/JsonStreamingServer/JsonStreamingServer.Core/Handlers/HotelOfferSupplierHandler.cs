using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Abstractions.Suppliers;
using JsonStreamingServer.Core.Extensions;
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
            .Select(s => GetEnumerator(request, s, cancellationToken))
            .ToList();

        try
        {
            var hasIncompleteEnumerator = false;

            do
            {
                hasIncompleteEnumerator = false;

                foreach (var streamEnumerator in streamEnumerators)
                {
                    Result<HotelOffer>? result;

                    try
                    {
                        if (await streamEnumerator.MoveNextAsync())
                        {
                            result = streamEnumerator.Current;
                            hasIncompleteEnumerator = true;
                        }
                        else
                        {
                            result = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new Result<HotelOffer>(ex);
                    }

                    if (result is not null)
                    {
                        yield return result.Value;
                    }
                }
            }
            while (hasIncompleteEnumerator);
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
            var supplierResults = GetSupplierResults(request, supplier, cancellationToken);

            await foreach (var supplierResult in supplierResults)
            {
                yield return supplierResult;
            }
        }
    }

    private static IAsyncEnumerator<Result<HotelOffer>> GetEnumerator(
        GetHotelOffersRequest request,
        IHotelOffersSupplier supplier,
        CancellationToken cancellationToken)
    {
        try
        {
            return supplier.GetHotelOffers(request, cancellationToken)
                .GetAsyncEnumerator(cancellationToken);
        }
        catch (Exception ex)
        {
            return new Result<HotelOffer>(ex)
                .ToAsyncEnumerable()
                .GetAsyncEnumerator(cancellationToken);
        }
    }

    private static IAsyncEnumerable<Result<HotelOffer>> GetSupplierResults(
        GetHotelOffersRequest request,
        IHotelOffersSupplier supplier,
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
