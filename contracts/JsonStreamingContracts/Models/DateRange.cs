namespace JsonStreaming.Contracts.Models;

public class DateRange
{
    public required DateOnly From { get; init; }
    public required DateOnly To { get; init; }
}
