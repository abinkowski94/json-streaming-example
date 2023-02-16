using JsonStreaming.Contracts.Models;
using JsonStreaming.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace JsonStreamingServer.WebApi.Controllers;

[ApiController]
[Route("hotels")]
public class HotelsController : ControllerBase
{
    [HttpGet(Name = "offers-stream")]
    public async IAsyncEnumerable<Response<HotelOffer>> GetHotelOffersAsync(CancellationToken cancellationToken)
    {
        yield break;
    }
}
