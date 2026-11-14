using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public record FindMostClosesLineCountByLonLatResponse(string  wkt  , string name , double DistanceByMeter)

{
    public static FindMostClosesLineCountByLonLatResponse FromAgg(LineAggregate agg, double DistanceByMeter) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeter, 2));
};