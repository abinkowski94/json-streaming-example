using System.Text.Json.Serialization;

namespace JsonStreaming.Contracts.Responses;

public class Error
{
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}