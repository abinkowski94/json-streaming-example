using System.Text.Json.Serialization;

namespace JsonStreaming.Contracts.Models;

public class HotelOffer
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
