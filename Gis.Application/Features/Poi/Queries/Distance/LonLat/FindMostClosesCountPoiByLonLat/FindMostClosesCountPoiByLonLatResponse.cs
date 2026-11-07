using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public record FindMostClosesCountPoiByLonLatResponse(string name, string wkt, double distanceByMeters)
{
    public static FindMostClosesCountPoiByLonLatResponse FromAgg(POIAggregate agg , double distanceByMeters) => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distanceByMeters,2));
}