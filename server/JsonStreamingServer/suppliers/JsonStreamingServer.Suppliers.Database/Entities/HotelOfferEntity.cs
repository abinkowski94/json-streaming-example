namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class HotelOfferEntity
{
    public HotelOfferEntity()
    {
        Images = new HashSet<ImageEntity>();
        Pricings = new HashSet<PricingEntity>();
    }

    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? ShortDescription { get; init; }
    public string? Description { get; init; }

    public ICollection<ImageEntity> Images { get; private init; }
    public ICollection<PricingEntity> Pricings { get; private init; }
}
