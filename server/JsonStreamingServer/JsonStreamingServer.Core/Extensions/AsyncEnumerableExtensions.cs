namespace JsonStreamingServer.Core.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T element)
    {
        yield return await new ValueTask<T>(element);
    }
}
