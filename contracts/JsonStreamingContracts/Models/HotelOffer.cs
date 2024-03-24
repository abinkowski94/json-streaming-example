namespace JsonStreaming.Contracts.Models;

public class HotelOffer
{
    public required string Id { get; init; }
    public required string Supplier { get; init; }

    public required DateOnly Day { get; init; }
    public required DateRange Avaliability { get; init; }
    public required Price TotalPrice { get; init; }


    public string? Name { get; init; }
    public string? ShortDescription { get; init; }
    public string? Description { get; init; }

    public IEnumerable<Image>? Images { get; init; }
    public IEnumerable<PriceBreakdownItem>? PriceBreakdown { get; init; }
}
