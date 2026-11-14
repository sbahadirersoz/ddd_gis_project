using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public record FindInRangeLineByLonLatResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindInRangeLineByLonLatResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};