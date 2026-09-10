using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public record FindMostClosesLineCountByLonLatResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindMostClosesLineCountByLonLatResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};