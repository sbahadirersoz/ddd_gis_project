using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiByName;

public record FindPoiByNameQueryResponse
{
    public WellKnownText WKT { get; }
    public PointDescription Desc { get; }
    public PointName Name { get; }
    public POIStatus Status { get; }

    private FindPoiByNameQueryResponse(WellKnownText wkt, PointDescription desc, PointName name, POIStatus status)
    {
        WKT = wkt;
        Desc = desc;
        Name = name;
        Status = status;
    }
    
    public static FindPoiByNameQueryResponse CreateFromAgg(POIAggregate aggregate)
    => new FindPoiByNameQueryResponse(aggregate.CoordinateValueObject.WKT,aggregate.PointDesc,aggregate.PointName,aggregate.Status);
};