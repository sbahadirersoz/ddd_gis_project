using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public record FindInRangeLineByWKTResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindInRangeLineByWKTResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};