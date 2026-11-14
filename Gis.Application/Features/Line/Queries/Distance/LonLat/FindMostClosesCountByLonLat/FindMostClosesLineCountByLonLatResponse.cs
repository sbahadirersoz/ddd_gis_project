using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public record FindMostClosesLineCountByLonLatResponse(string  wkt  , string name , double DistanceByMeters)

{
    public static FindMostClosesLineCountByLonLatResponse FromAgg(LineAggregate agg, double DistanceByMeters) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(DistanceByMeters, 2));
};