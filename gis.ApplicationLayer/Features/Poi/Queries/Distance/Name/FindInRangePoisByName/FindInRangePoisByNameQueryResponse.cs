using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindInRangePoisByName;

public record FindInRangePoisByNameQueryResponse(string wkt, string name,double distanceByMeters)
{
    public static FindInRangePoisByNameQueryResponse FromAgg(POIAggregate agg,double distance) =>
        new(agg.CoordinateValueObject.WKT.Value, agg.PointName.Value,Math.Round(distance , 2));
}