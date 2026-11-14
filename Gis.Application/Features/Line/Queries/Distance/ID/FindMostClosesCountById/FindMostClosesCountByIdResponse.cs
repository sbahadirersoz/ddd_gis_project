using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public record FindMostClosesCountByIdResponse
    (string wkt, string name, double DistanceByMeter)
{
    public static FindMostClosesCountByIdResponse FromAgg(
        LineAggregate agg, double DistanceByMeter) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter,2));
}
