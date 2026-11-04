using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public record FindInRangeLinesByIdResponse(string wkt, string name, double distance)
{
    public static FindInRangeLinesByIdResponse FromAgg(
        LineAggregate agg, double distance) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
}
