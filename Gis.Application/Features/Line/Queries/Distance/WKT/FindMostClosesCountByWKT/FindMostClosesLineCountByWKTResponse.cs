using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public record FindMostClosesLineCountByWKTResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindMostClosesLineCountByWKTResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};