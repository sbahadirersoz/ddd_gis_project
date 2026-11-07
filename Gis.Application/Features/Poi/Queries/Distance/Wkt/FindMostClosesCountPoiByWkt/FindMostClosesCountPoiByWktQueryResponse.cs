using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public record FindMostClosesCountPoiByWktQueryResponse(string name, string wkt, double DistanceByMeters)
{
    public static FindMostClosesCountPoiByWktQueryResponse FromAgg(POIAggregate agg ,double distanceByMeters)
    => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value ,Math.Round(distanceByMeters,2));
}