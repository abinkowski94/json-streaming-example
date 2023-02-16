namespace JsonStreamingServer.Core.Models.Domain;

public class PriceBreakdownItem
{
    public required Price Price { get; set; }

    public required AgeRange AgeRange { get; set; }
}
