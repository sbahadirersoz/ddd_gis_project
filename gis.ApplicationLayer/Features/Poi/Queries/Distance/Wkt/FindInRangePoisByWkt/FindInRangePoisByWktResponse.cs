using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public record FindInRangePoisByWktResponse(string name, string wkt,double distanceBYMeters)
{

    public static FindInRangePoisByWktResponse FromAgg(POIAggregate agg,double distance) =>
        new FindInRangePoisByWktResponse(agg.PointName.Value, agg.CoordinateValueObject.WKT.Value,distance);
}