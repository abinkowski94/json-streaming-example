namespace JsonStreaming.Contracts.Models;

public class HotelOffer
{
    public required string Id { get; set; }

    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public required DateOnly Day { get; set; }
    public required DateRange Avaliability { get; set; }
    public required Price TotalPrice { get; set; }

    public IEnumerable<Image>? Images { get; set; }
    public IEnumerable<PriceBreakdownItem>? PriceBreakdown { get; set; }
}
