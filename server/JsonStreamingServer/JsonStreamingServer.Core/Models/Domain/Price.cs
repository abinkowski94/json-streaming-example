namespace JsonStreamingServer.Core.Models.Domain;

public class Price
{
    public required decimal Value { get; set; }
    public required string Currency { get; set; }
}
