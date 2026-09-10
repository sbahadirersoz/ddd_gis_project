using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public record FindInRangePoisByLonLatResponse(string name, string wkt, double distance)
{
    public static FindInRangePoisByLonLatResponse FromAgg(POIAggregate agg , double distance) => new FindInRangePoisByLonLatResponse(agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distance,2));
}