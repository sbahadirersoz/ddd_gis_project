using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public record CreatePoiCommandResponse
{
    private CreatePoiCommandResponse(WellKnownText wkt, Coordinates coordinates, PointDescription description, POIStatus status)
    {
        WKT = wkt;
        Coordinates = coordinates;
        Description = description;
        Status = status;
    }

    public WellKnownText WKT { get; }

    public Coordinates Coordinates { get; }
    public PointDescription Description { get; }
    public POIStatus Status { get; }
    
    public static CreatePoiCommandResponse CreateFromAgg(POIAggregate aggregate)
    {
        return new CreatePoiCommandResponse
        (
            aggregate.Coordinates.WKT,
            aggregate.Coordinates,
            aggregate.PointDesc,
            aggregate.Status
        );
    }
}
