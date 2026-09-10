using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public record FindMostClosesCountPoiByWktQueryResponse(string name, string wkt, double distance)
{
    public static FindMostClosesCountPoiByWktQueryResponse FromAgg(POIAggregate agg ,double distance)
    => new (agg.PointName.Value,agg.CoordinateValueObject.WKT.Value ,Math.Round(distance,2));
}