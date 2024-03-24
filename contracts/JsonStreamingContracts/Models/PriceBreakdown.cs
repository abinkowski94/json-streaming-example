namespace JsonStreaming.Contracts.Models;

public class PriceBreakdownItem
{
    public required Price Price { get; init; }

    public required AgeRange AgeRange { get; init; }
}
