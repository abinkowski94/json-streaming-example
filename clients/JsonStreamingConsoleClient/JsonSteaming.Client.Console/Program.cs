using JsonSteaming.Client.Console;
using JsonStreaming.Contracts.Models;
using JsonStreaming.Contracts.Responses;
using System.Text.Json;

using var client = new HttpClient
{
    BaseAddress = new Uri("https://localhost:5270")
};

using var httpRequest = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("hotels/offers-stream?mix-supplier-offers=true", UriKind.Relative)
};

using var httpResponse = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

var jsonResponseStream = JsonSerializer.DeserializeAsyncEnumerable<Response<HotelOffer>>(responseStream, Consts.StreamingSerializerOptions);
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