using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public record FindClosestPoiByLonLatResponse(string name, string wkt,  double distanceByMeters)
{
    public static FindClosestPoiByLonLatResponse FromAgg(POIAggregate agg , double distanceByMeters) => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distanceByMeters,2));
}