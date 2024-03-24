using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Core.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T element)
    {
        yield return await new ValueTask<T>(element);
    }

    public static async IAsyncEnumerable<Result<T>> ConcatResultAsyncEnumerables<T>(
        this IEnumerable<IAsyncEnumerable<Result<T>>> streams,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var streamEnumerators = streams
            .Select(s => s.GetResultAsyncEnumerator(cancellationToken))
            .ToList();

        try
        {
            foreach (var streamEnumerator in streamEnumerators)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Result<T>? result = await streamEnumerator.GetResultAsync();

                while (result is not null)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    yield return result.Value;

                    result = await streamEnumerator.GetResultAsync();
                }
            }
        }
        finally
        {
            foreach (var streamEnumerator in streamEnumerators)
            {
                await streamEnumerator.DisposeAsync();
            }
        }
    }

    public static async IAsyncEnumerable<Result<T>> ZipResultAsyncEnumerables<T>(
        this IEnumerable<IAsyncEnumerable<Result<T>>> streams,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var streamEnumerators = streams
            .Select(s => s.GetResultAsyncEnumerator(cancellationToken))
            .ToList();

        try
        {
            var hasIncompleteEnumerator = true;

            while (hasIncompleteEnumerator)
            {
                cancellationToken.ThrowIfCancellationRequested();
                hasIncompleteEnumerator = false;

                foreach (var streamEnumerator in streamEnumerators)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var result = await streamEnumerator.GetResultAsync();
                    if (result is not null)
                    {
                        hasIncompleteEnumerator = true;
                        yield return result.Value;
                    }
                }
            }
        }
        finally
        {
            foreach (var streamEnumerator in streamEnumerators)
            {
                await streamEnumerator.DisposeAsync();
            }
        }
    }

    private static IAsyncEnumerator<Result<T>> GetResultAsyncEnumerator<T>(
        this IAsyncEnumerable<Result<T>> enumerable,
        CancellationToken cancellationToken)
    {
        try
        {
            return enumerable
                .GetAsyncEnumerator(cancellationToken);
        }
        catch (Exception ex)
        {
            return new Result<T>(ex)
                .ToAsyncEnumerable()
                .GetAsyncEnumerator(cancellationToken);
        }
    }

    private static async ValueTask<Result<T>?> GetResultAsync<T>(
        this IAsyncEnumerator<Result<T>> streamEnumerator)
    {
        Result<T>? result;

        try
        {
            if (await streamEnumerator.MoveNextAsync())
            {
                result = streamEnumerator.Current;
            }
            else
            {
                result = null;
            }
        }
        catch (Exception ex)
        {
            result = new Result<T>(ex);
        }

        return result;
    }
}
