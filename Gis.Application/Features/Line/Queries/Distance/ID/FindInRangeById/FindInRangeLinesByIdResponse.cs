using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public record FindInRangeLinesByIdResponse(string wkt, string name, double DistanceByMeters)
{
    public static FindInRangeLinesByIdResponse FromAgg(
        LineAggregate agg, double DistanceByMeters) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeters, 2));
}
