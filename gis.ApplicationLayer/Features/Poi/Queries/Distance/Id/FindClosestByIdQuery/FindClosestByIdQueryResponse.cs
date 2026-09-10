using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindNearestPoi;

public record FindClosestByIdQueryResponse(string wkt , string name,double distance)
{
    public static FindClosestByIdQueryResponse FromAggregate(POIAggregate findNearestSameEntityByGivenIdAsync,double distance)
        => new(findNearestSameEntityByGivenIdAsync.CoordinateValueObject.WKT.Value,findNearestSameEntityByGivenIdAsync.PointName.Value,Math.Round(distance,2));
}