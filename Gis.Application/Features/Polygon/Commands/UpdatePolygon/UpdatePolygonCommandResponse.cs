using gis.Domain.Aggregates;
using gis.Domain.Entities.Information.Polygon;

namespace Gis.Application.Features.Polygon.Commands.UpdatePolygon;

public record UpdatePolygonCommandResponse(Guid id, string name, string? desc, PolygonStatus status)

{
    public static UpdatePolygonCommandResponse FromAgg(PolygonAggregate agg)
    {
        return new UpdatePolygonCommandResponse(agg.Id.Value, agg.Name.Value, agg.Description.Value, agg.Status);
    }
};
