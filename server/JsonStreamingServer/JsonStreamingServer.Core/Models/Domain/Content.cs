namespace JsonStreamingServer.Core.Models.Domain;

public class Content
{
    public IList<Image>? Images { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }
}
