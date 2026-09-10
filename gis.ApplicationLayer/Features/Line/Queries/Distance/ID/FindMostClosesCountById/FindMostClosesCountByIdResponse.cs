using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public record FindMostClosesCountByIdResponse
    (string wkt, string name, double distance)
{
    public static FindMostClosesCountByIdResponse FromAgg(
        LineAggregate agg, double distance) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance,2));
}
