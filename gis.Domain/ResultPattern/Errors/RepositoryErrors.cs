namespace gis.Domain.ResultPattern.Errors;

public static  class RepositoryErrors
{
    public static readonly Error VALUE_ALREADY_EXIST_IN_DB = new Error("RepositoryErrors.Entity.AlreadyExist", "ENTITY_ALREADY_EXIST_IN_DB");
    public static readonly Error ENTITY_NOT_FOUND = new Error("RepositoryErrors.Entity.NotFound", "ENTITY_NOT_FOUND");
}