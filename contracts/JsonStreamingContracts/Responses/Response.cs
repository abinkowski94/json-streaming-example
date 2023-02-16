using System.Text.Json.Serialization;

namespace JsonStreaming.Contracts.Responses;

public class Response<T>
{
    [JsonPropertyName("value")]
    public T? Value { get; init; }

    [JsonPropertyName("error")]
    public Error? Error { get; init; }
}
