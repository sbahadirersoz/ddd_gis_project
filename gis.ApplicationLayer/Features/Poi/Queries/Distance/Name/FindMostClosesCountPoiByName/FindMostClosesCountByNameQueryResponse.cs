using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

public record FindMostClosesCountByNameQueryResponse(string Name, string wkt,double DistanceByMeters)
{
    public static FindMostClosesCountByNameQueryResponse FromAgg(POIAggregate agg,double distance)
        => new(agg.PointName.Value, agg.CoordinateValueObject.WKT.Value,Math.Round(distance,2));
}