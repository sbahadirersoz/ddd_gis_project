using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public record FindMostClosesCountPoiByLonLatResponse(string name, string wkt, double distance)
{
    public static FindMostClosesCountPoiByLonLatResponse FromAgg(POIAggregate agg , double distance) => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distance,2));
}