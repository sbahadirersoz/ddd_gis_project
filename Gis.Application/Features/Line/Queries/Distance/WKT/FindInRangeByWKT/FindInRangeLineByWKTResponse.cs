using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public record FindInRangeLineByWKTResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindInRangeLineByWKTResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};