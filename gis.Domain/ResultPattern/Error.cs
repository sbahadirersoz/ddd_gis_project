namespace gis.Domain.ResultPattern;

public sealed record Error(string Code, string Desc, string? StackTrace = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error UnExpected(string code, string desc)
        => new(code, desc, Environment.StackTrace);

}