using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public record FindInRangePoisByWktResponse(string name, string wkt,double distanceByMeters)
{

    public static FindInRangePoisByWktResponse FromAgg(POIAggregate agg,double distanceByMeters) =>
        new FindInRangePoisByWktResponse(agg.PointName.Value, agg.CoordinateValueObject.WKT.Value,distanceByMeters);
}