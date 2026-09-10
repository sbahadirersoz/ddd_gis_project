using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public record FindClosestLineByLonLatResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindClosestLineByLonLatResponse FromAgg(
        LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};