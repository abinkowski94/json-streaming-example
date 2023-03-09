namespace JsonStreamingServer.Core.Models.Requests;

public class GetHotelOffersRequest
{
    public bool MixSupplierOffers { get; set; }

    public uint? MaxResults { get; set; }

    public double? ErrorChance { get; set; }
}