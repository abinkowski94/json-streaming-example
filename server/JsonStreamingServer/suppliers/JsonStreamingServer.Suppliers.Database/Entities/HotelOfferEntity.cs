namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class HotelOfferEntity
{
    public HotelOfferEntity()
    {
        Images = new HashSet<ImageEntity>();
        Pricings = new HashSet<PricingEntity>();
    }

    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public ICollection<ImageEntity> Images { get; private set; }
    public ICollection<PricingEntity> Pricings { get; private set; }
}
