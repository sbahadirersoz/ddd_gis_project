using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public record FindClosestLineByLonLatResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindClosestLineByLonLatResponse FromAgg(
        LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};