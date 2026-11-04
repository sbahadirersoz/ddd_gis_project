using gis.Domain.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.IDs;

public record PolygonID : EntityId
{
    
    public PolygonID(Guid id) : base(id)
    {}
    public static PolygonID New() => new(Guid.NewGuid());

    public static Result<PolygonID> FromGuid(Guid value)
    {
        return value == Guid.Empty ? Result<PolygonID>.Failure(DomainErrors.EMPTY_ID_FORMAT) : Result<PolygonID>.Success(new PolygonID(value));
    }
}
