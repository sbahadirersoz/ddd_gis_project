using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

public record FindClosestPoiByWktResponse(string name,string  wkt,double distanceByMeters )
{
    public static FindClosestPoiByWktResponse CreateFromAgg(POIAggregate agg,double distanceByMeters) =>
        new(agg.PointName.Value, agg.CoordinateValueObject.WKT.Value,distanceByMeters);
}