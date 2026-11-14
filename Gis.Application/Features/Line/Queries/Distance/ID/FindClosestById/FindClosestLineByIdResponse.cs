using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public record FindClosestLineByIdResponse(string wkt, string name, double DistanceByMeter)
{
    public static  FindClosestLineByIdResponse FromAgg(LineAggregate agg,  double DistanceByMeter) => new (agg.WKT.Value,agg.LineName.Value,Math.Round(DistanceByMeter,2));
}