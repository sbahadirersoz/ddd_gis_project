using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindInRangeByName;

public record FindInRangeByNameResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindInRangeByNameResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};