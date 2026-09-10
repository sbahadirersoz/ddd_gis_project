using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public record FindClosestLineByWKTResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindClosestLineByWKTResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};