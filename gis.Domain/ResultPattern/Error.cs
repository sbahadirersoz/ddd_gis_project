namespace gis.Domain.ResultPattern;

public sealed record Error(string Code, string Desc, ErrorType type =ErrorType.VALIDATION)
{
    
    public static readonly Error None = new(string.Empty, string.Empty);

}