using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindInRangeByName;

public record FindInRangeByNameResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindInRangeByNameResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};