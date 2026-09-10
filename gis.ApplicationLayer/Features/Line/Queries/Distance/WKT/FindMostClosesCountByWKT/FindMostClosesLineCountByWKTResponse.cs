using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public record FindMostClosesLineCountByWKTResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindMostClosesLineCountByWKTResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};