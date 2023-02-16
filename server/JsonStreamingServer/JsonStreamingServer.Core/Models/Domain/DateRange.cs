namespace JsonStreamingServer.Core.Models.Domain;

public class DateRange
{
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
}
