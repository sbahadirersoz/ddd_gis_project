namespace gis.Domain.ResultPattern.Errors;

public static class DomainServiceErrors
{
    public static readonly Error SAME_CREDENTIALS_FOR_UPDATING  =
        new Error("POI.Updating.SameValues", "The provided updates are the same as the current ones.");

}