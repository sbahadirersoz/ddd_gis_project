using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public record FindMostClosesCountByNameResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindMostClosesCountByNameResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};