namespace JsonStreamingServer.Suppliers.Database.Entities;

internal class PricingEntity
{
    public required DateOnly DayFrom { get; init; }
    public required DateOnly DayTo { get; init; }
    public required uint AgeFrom { get; init; }
    public required uint AgeTo { get; init; }
    public required decimal Value { get; init; }
    public required string Currency { get; init; }

    public HotelOfferEntity? OfferEntity { get; private init; }
}
