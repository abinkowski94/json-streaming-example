namespace JsonStreaming.Contracts.Models;

public class Price
{
    public required decimal Value { get; init; }
    public required string Currency { get; init; }
}
