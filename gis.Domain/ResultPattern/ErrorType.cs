namespace gis.Domain.ResultPattern;

public enum ErrorType
{
    VALIDATION=400,
    NOT_FOUND = 404,
    CONFLICT= 409,
    UNEXPECTED = 500
}