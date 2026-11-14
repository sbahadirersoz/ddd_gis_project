using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public record FindClosestLineByIdResponse(string wkt, string name, double DistanceByMeters)
{
    public static  FindClosestLineByIdResponse FromAgg(LineAggregate agg,  double DistanceByMeters) => new (agg.WKT.Value,agg.LineName.Value,Math.Round(DistanceByMeters,2));
}