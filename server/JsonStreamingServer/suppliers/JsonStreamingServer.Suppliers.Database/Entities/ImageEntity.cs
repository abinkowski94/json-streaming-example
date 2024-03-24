namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class ImageEntity
{
    public required string Url { get; init; }
    public string? Caption { get; init; }

    public HotelOfferEntity? OfferEntity { get; private init; }
}
