using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace JsonStreamingServer.Core.Handlers;

public partial class HotelOfferExternalIdGeneatingHandler : AbstractHotelOfferStreamHandler
{
    public HotelOfferExternalIdGeneatingHandler(IHotelOfferStreamHandler nextHandler)
    : base(nextHandler)
    {
    }


    public override async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
        GetHotelOffersRequest request,
        IAsyncEnumerable<Result<HotelOffer>> inputStream,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var offerResult in inputStream)
        {
            if (offerResult.HasValue)
            {
                GenerateExternalId(offerResult.Value!);
            }

            yield return offerResult;
        }
    }

    private static void GenerateExternalId(HotelOffer offer)
    {
        offer.ExternalId = GuidSpecialCharsRegex()
            .Replace(Convert.ToBase64String(offer.Id.ToByteArray()), string.Empty);
    }

    [GeneratedRegex("[/+=]")]
    private static partial Regex GuidSpecialCharsRegex();
}
