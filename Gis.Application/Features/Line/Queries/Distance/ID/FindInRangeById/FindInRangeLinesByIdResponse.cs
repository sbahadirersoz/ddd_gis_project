using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public record FindInRangeLinesByIdResponse(string wkt, string name, double DistanceByMeter)
{
    public static FindInRangeLinesByIdResponse FromAgg(
        LineAggregate agg, double DistanceByMeter) => new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
}
