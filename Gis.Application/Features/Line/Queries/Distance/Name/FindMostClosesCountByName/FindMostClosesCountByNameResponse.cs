using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public record FindMostClosesCountByNameResponse(string  wkt  , string name , double DistanceInMeter)

{
    public static FindMostClosesCountByNameResponse FromAgg(LineAggregate agg, double distance) =>
        new(agg.WKT.Value, agg.LineName.Value, Math.Round(distance, 2));
};