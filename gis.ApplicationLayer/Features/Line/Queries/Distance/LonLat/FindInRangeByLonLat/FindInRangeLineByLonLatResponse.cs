using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public record FindInRangeLineByLonLatResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindInRangeLineByLonLatResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};