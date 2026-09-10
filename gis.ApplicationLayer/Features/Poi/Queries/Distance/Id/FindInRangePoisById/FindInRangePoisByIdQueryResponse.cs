using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public record FindInRangePoisByIdQueryResponse(string name,Guid id ,string wkt,double DistanceByMeters  )

{
    public static FindInRangePoisByIdQueryResponse FromAgg(POIAggregate agg,double distance)
        => new(agg.PointName.Value, agg.Id.Value, agg.CoordinateValueObject.WKT.Value,Math.Round(distance,2));
}