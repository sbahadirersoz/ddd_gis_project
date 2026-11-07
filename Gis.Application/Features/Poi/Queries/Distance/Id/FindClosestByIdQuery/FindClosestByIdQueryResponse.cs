using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.FindNearestPoi;

public record FindClosestByIdQueryResponse(string wkt , string name,double distanceByMeters)
{
    public static FindClosestByIdQueryResponse FromAggregate(POIAggregate findNearestSameEntityByGivenIdAsync,double distanceByMeters)
        => new(findNearestSameEntityByGivenIdAsync.CoordinateValueObject.WKT.Value,findNearestSameEntityByGivenIdAsync.PointName.Value,Math.Round(distanceByMeters,2));
}