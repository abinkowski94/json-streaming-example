namespace JsonStreaming.Contracts.Models;

public class PriceBreakdownItem
{
    public required Price Price { get; set; }

    public required AgeRange AgeRange { get; set; }
}
