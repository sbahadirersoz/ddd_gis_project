using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public record CreatePoiCommandResponse
{
    private CreatePoiCommandResponse(WellKnownText wkt, CoordinateValueObject coordinateValueObject, PointDescription description, POIStatus status)
    {
        WKT = wkt;
        CoordinateValueObject = coordinateValueObject;
        Description = description;
        Status = status;
    }

    public WellKnownText WKT { get; }

    public CoordinateValueObject CoordinateValueObject { get; }
    public PointDescription Description { get; }
    public POIStatus Status { get; }
    
    public static CreatePoiCommandResponse CreateFromAgg(POIAggregate aggregate)
    {
        return new CreatePoiCommandResponse
        (
            aggregate.CoordinateValueObject.WKT,
            aggregate.CoordinateValueObject,
            aggregate.PointDesc,
            aggregate.Status
        );
    }
}
