using JsonStreaming.Contracts.Models;
using JsonStreaming.Contracts.Responses;

using var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5270")
};

using var httpRequest = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("hotels/offers-stream", UriKind.Relative)
};

using var httpResponse = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

var jsonResponseStream = System.Text.Json.JsonSerializer.DeserializeAsyncEnumerable<Response<HotelOffer>>(responseStream);

await foreach(var response in jsonResponseStream)
{
    if (response?.Value is not null)
    {
        Console.WriteLine($"{response.Value.Id} -  {response.Value.Name}"); 
    }
    else if (response?.Error is not null)
    {
        Console.WriteLine($"There was an error: {response.Error.Message}");
    }
}