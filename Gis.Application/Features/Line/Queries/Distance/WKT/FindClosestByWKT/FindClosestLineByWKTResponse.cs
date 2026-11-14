using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public record FindClosestLineByWKTResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindClosestLineByWKTResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};