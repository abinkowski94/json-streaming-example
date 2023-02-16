using JsonStreamingServer.Core.Abstractions.Services;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonStreamingServer.Core.Services;

internal class HotelService : IHotelService
{
    public async IAsyncEnumerable<Result<HotelOffer>> GetHotelOffers(GetHotelOffersRequest request, CancellationToken cancellationToken)
    {
        yield break;
    }
}
