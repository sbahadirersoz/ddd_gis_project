using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public record FindClosestPoiByLonLatResponse(string name, string wkt, double distance)
{
    public static FindClosestPoiByLonLatResponse FromAgg(POIAggregate agg , double distance) => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value,Math.Round(distance,2));
}