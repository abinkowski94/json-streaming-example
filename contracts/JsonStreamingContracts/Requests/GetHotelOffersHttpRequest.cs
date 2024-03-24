using Microsoft.AspNetCore.Mvc;

namespace JsonStreaming.Contracts.Requests;

public class GetHotelOffersHttpRequest
{
    [FromQuery(Name = "mix-supplier-offers")]
    public bool MixSupplierOffers { get; init; }

    [FromQuery(Name = "max-results")]
    public uint? MaxResults { get; init; }

    [FromQuery(Name = "error-chance")]
    public double? ErrorChance { get; init; }
}
