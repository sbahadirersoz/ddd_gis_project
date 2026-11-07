using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public record FindInRangePoisByLonLatResponse(string name, string wkt, double distanceByMeters)
{
    public static FindInRangePoisByLonLatResponse FromAgg(POIAggregate agg , double distanceByMeters) => new FindInRangePoisByLonLatResponse(agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distanceByMeters,2));
}