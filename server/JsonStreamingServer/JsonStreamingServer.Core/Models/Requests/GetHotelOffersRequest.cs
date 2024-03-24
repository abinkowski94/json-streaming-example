namespace JsonStreamingServer.Core.Models.Requests;

public class GetHotelOffersRequest
{
    public bool MixSupplierOffers { get; init; }

    public uint? MaxResults { get; init; }

    public double? ErrorChance { get; init; }
}