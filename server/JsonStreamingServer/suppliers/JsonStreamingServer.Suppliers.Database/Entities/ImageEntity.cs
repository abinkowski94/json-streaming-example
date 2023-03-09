namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class ImageEntity
{
    public required string Url { get; set; }
    public string? Caption { get; set; }

    public HotelOfferEntity? OfferEntity { get; private set; }
}
