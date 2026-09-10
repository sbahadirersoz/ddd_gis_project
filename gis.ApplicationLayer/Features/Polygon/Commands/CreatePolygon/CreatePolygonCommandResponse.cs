using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Polygon.Commands;

public record CreatePolygonCommandResponse(string WKT, string name, Guid id)
{

    public string name { get;  } = name;
    public string WKT { get;  } = WKT;
    public Guid id { get;  } = id;
    public static CreatePolygonCommandResponse Create(PolygonAggregate agg) => new (agg.Name.Value,agg.Shell.wkt.Value,agg.Id.Value);
}