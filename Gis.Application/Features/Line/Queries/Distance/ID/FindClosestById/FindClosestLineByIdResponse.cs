using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public record FindClosestLineByIdResponse(string wkt, string name, double distance)
{
    public static  FindClosestLineByIdResponse FromAgg(LineAggregate agg,  double distance) => new (agg.WKT.Value,agg.LineName.Value,Math.Round(distance,2));
}