using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public record FindMostClosesCountByIdResponse
    (string wkt, string name, double DistanceByMeters)
{
    public static FindMostClosesCountByIdResponse FromAgg(
        LineAggregate agg, double DistanceByMeters) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeters,2));
}
