using gis.Domain.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.IDs;

public record LineID : EntityId
{
    private LineID(Guid id) : base(id)
    {
    }
    public static LineID New() => new(Guid.NewGuid());

    public static Result<LineID> FromGuid(Guid value)
    {
        return value == Guid.Empty ? Result<LineID>.Failure(DomainErrors.POIErrors.PointID.EMPTY_ID_FORMAT) : Result<LineID>.Success(new LineID(value));
    }
}