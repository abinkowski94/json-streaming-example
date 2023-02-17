using JsonStreaming.Contracts.Models;
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
    public async IAsyncEnumerable<Response<HotelOffer>> GetHotelOffersAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var result in _hotelService.GetHotelOffers(new GetHotelOffersRequest(), cancellationToken))
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

    private static HotelOffer MapOffer(Core.Models.Domain.HotelOffer offer)
    {
        return new HotelOffer
        {
            Id = offer.ExternalId,
            Name = offer.Name,
        };
    }

    private static Error MapError<T>(Result<T> result)
    {
        return new Error { Message = result.Error!.Message };
    }
}

