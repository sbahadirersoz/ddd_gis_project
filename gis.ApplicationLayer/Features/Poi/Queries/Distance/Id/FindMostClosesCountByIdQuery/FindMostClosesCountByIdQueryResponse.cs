using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public record FindMostClosesCountByIdQueryResponse
{
    public string wkt { get;  }
    public string name { get;  }
    public double distance { get;  }

    private FindMostClosesCountByIdQueryResponse(string wkt, string name,double distance)
    {
        this.wkt = wkt;
        this.name = name;
        this.distance = distance;
    }

    public static FindMostClosesCountByIdQueryResponse CreateFromAgg(POIAggregate aggregate,double distance)
    {
        return new FindMostClosesCountByIdQueryResponse(aggregate.CoordinateValueObject.WKT.Value,
            aggregate.PointName.Value,distance);
    }
}