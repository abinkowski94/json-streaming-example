using JsonStreaming.Contracts.Models;
using JsonStreaming.Contracts.Responses;
using System.Text.Json;
using System.Text.Json.Serialization;

using var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5270")
};

using var httpRequest = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("hotels/offers-stream?mix-supplier-offers=true", UriKind.Relative)
};

using var httpResponse = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

var jsonResponseStream = JsonSerializer.DeserializeAsyncEnumerable<Response<HotelOffer>>(responseStream, new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
});

var offerNumber = 1;

await foreach(var response in jsonResponseStream)
{
    Console.Write($"no. {offerNumber++} | ");

    if (response?.Value is not null)
    {
        Console.Write($"supplier: {response.Value.Supplier} | ");

        Console.Write(response.Value.Id);
        Console.Write(" - ");
        Console.WriteLine(response.Value.Name);
    }
    else if (response?.Error is not null)
    {
        Console.WriteLine($"There was an error: {response.Error.Message}");
    }
}