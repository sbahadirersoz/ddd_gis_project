using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;

public record FindPoiByIdQueryResponse
{
    public WellKnownText WKT { get; }
    public PointDescription Desc { get; }
    public PointName Name { get; }
    public POIStatus Status { get; }

    private FindPoiByIdQueryResponse(WellKnownText wkt, PointDescription desc, PointName name, POIStatus status)
    {
        WKT = wkt;
        Desc = desc;
        Name = name;
        Status = status;
    }
    public static FindPoiByIdQueryResponse CreateFromAgg(POIAggregate agg)
        => new FindPoiByIdQueryResponse
        (
            agg.CoordinateValueObject.WKT,
            agg.PointDesc, agg.PointName,
            agg.Status
        );
}