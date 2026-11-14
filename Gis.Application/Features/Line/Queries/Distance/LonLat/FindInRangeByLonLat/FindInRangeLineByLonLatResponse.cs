using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public record FindInRangeLineByLonLatResponse(string  wkt  , string name , double DistanceByMeters)

{
    public static FindInRangeLineByLonLatResponse FromAgg(LineAggregate agg, double DistanceByMeters) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeters, 2));
};