namespace JsonStreamingServer.Core.Models.Results;

public readonly struct Result<T>
{
    public T? Value { get; }

    public Exception? Error { get; }

    public bool HasError => Error is not null;

    public bool HasValue => Value is not null;

    public Result(T value)
    {
        Value = value;
    }

    public Result(Exception error)
    {
        Error = error;
    }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Exception error)
    {
        return new Result<T>(error);
    }
}
