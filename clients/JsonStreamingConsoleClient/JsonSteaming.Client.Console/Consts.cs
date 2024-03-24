using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonSteaming.Client.Console;

internal static class Consts
{
    public static readonly JsonSerializerOptions StreamingSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };
}
