using gis.Domain.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.IDs;

public record PointID : EntityId

{
    private PointID(Guid id) : base(id)
    {
    }

    public static PointID New() => new(Guid.NewGuid());

    public static Result<PointID> FromGuid(Guid value)
    {
        return value == Guid.Empty ? Result<PointID>.Failure(DomainErrors.PointOfInterestErrors.PointIDErrors.EMPTY_ID_FORMAT) : Result<PointID>.Success(new PointID(value));
    }
}