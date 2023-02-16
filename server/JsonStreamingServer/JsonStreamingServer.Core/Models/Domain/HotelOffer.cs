namespace JsonStreamingServer.Core.Models.Domain;

public class HotelOffer
{
    public required Guid Id { get; set; }

    public required DateRange Avaliability { get; set; }
    public required Price TotalPrice { get; set; }

    public string? ExternalId { get; set; }
    public string? Name { get; set; }
    public Content? Content { get; set; }
    public List<PriceBreakdownItem>? PriceBreakdown { get; set; } 
}
