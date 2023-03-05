namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class PricingEntity
{
    public required DateOnly DayFrom { get; set; }
    public required DateOnly DayTo { get; set; }
    public required uint AgeFrom { get; set; }
    public required uint AgeTo { get; set; }
    public required decimal Value { get; set; }
    public required string Currency { get; set; }

    public HotelOfferEntity? OfferEntity { get; private set; }
}
