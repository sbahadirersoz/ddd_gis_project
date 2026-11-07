using gis.Domain.Aggregates;

namespace Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public record FindMostClosesCountByIdQueryResponse
{
    public string wkt { get;  }
    public string name { get;  }
    public double distanceByMeters { get;  }

    private FindMostClosesCountByIdQueryResponse(string wkt, string name,double distanceByMeters)
    {
        this.wkt = wkt;
        this.name = name;
        this.distanceByMeters = distanceByMeters;
    }

    public static FindMostClosesCountByIdQueryResponse CreateFromAgg(POIAggregate aggregate,double distanceByMeters)
    {
        return new FindMostClosesCountByIdQueryResponse(aggregate.CoordinateValueObject.WKT.Value,
            aggregate.PointName.Value,Math.Round(distanceByMeters,2));
    }
}