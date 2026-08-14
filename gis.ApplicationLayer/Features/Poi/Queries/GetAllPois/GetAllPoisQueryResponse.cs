using gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;
using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

namespace gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;

public record GetAllPoisQueryResponse
{
    public WellKnownText WKT { get; }
    public PointDescription Desc { get; }
    public PointName Name { get; }
    public POIStatus Status { get; }
    
    private GetAllPoisQueryResponse(WellKnownText wkt, PointDescription desc, PointName name, POIStatus status)
    {
        WKT = wkt;
        Desc = desc;
        Name = name;
        Status = status;
    }
    public static GetAllPoisQueryResponse CreateFromAgg(POIAggregate agg)
        => new GetAllPoisQueryResponse
        (
            agg.CoordinateValueObject.WKT,
            agg.PointDesc, agg.PointName,
            agg.Status
        );
};