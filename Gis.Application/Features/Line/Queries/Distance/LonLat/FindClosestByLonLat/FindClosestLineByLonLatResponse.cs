using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public record FindClosestLineByLonLatResponse(string  wkt  , string name , double DistanceByMeters)

{
    public static FindClosestLineByLonLatResponse FromAgg(
        LineAggregate agg, double DistanceByMeters) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeters, 2));
};