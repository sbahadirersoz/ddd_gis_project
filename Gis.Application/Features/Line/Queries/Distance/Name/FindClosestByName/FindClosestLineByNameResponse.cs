using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindClosestByName;

public record FindClosestLineByNameResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindClosestLineByNameResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};