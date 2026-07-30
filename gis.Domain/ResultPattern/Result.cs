namespace gis.Domain.ResultPattern;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Geçersiz Result durumu.");
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}
public class Result<T> : Result
{
    public T? Value { get; }
    private Result(bool isSuccess, T? value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }
    public static Result<T> Success(T value) => new(true, value, Error.None);
    public new static Result<T> Failure(Error error) => new(false, default, error);
}
public sealed record Error(string Code, string Desc, string? StackTrace = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error UnExpected(string code, string desc)
        => new(code, desc, Environment.StackTrace);

}