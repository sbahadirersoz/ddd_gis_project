using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindClosestByName;

public record FindClosestLineByNameResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindClosestLineByNameResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};