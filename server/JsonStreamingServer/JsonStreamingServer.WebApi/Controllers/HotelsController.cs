using JsonStreaming.Contracts.Models;
using JsonStreaming.Contracts.Requests;
using JsonStreaming.Contracts.Responses;
using JsonStreamingServer.Core.Abstractions.Services;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.WebApi.Controllers;

[ApiController]
[Route("hotels")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet("offers-stream")]
    public async IAsyncEnumerable<Response<HotelOffer>> GetHotelOffersAsync(
        [FromQuery] GetHotelOffersHttpRequest httpRequest,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = CreateRequest(httpRequest);

        await foreach (var result in _hotelService.GetHotelOffers(request, cancellationToken))
        {
            if (result.HasValue)
            {
                yield return new Response<HotelOffer>
                {
                    Value = MapOffer(result.Value!)
                };
            }
            else
            {
                yield return new Response<HotelOffer>
                {
                    Error = MapError(result)
                };
            }
        }
    }

    private static GetHotelOffersRequest CreateRequest(GetHotelOffersHttpRequest httpRequest) => new()
    {
        MixSupplierOffers = httpRequest.MixSupplierOffers,
        MaxResults = httpRequest.MaxResults,
        ErrorChance = httpRequest.ErrorChance,
    };

    private static HotelOffer MapOffer(Core.Models.Domain.HotelOffer offer) => new()
    {
        Id = offer.ExternalId!,
        Supplier = offer.Supplier,
        Name = offer.Name,
        ShortDescription = offer.Content?.ShortDescription,
        Description = offer.Content?.Description,
        Day = offer.PriceBreakdown?.FirstOrDefault()?.Day
                ?? offer.Avaliability.From,
        Avaliability = new DateRange
        {
            From = offer.Avaliability.From,
            To = offer.Avaliability.To,
        },
        TotalPrice = new Price
        {
            Currency = offer.TotalPrice.Currency,
            Value = offer.TotalPrice.Value,
        },
        Images = offer.Content?.Images?.Select(i => new Image
        {
            Url = i.Url,
            Caption = i.Caption,
        }),
        PriceBreakdown = offer.PriceBreakdown?.Select(b => new PriceBreakdownItem
        {
            Price = new Price
            {
                Currency = b.Price.Currency,
                Value = b.Price.Value,
            },
            AgeRange = new AgeRange
            {
                From = b.AgeRange.From,
                To = b.AgeRange.To,
            },
        }),
    };

    private static Error MapError<T>(Result<T> result) => new()
    {
        Message = result.Error!.Message,
    };
}

