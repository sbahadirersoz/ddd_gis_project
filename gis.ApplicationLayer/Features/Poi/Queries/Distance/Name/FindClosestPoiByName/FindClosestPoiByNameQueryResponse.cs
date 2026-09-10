using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestPoiByName;

public record FindClosestPoiByNameQueryResponse(string wkt, string name,double distanceBYMeters)
{
    public static FindClosestPoiByNameQueryResponse FromAgg(POIAggregate aggregate,double distance ) =>
        new(aggregate.CoordinateValueObject.WKT.Value, aggregate.PointName.Value,Math.Round(distance,2));
};