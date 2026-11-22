using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.Information.Polygon;

using Gis.Infrastructure.HelperMethods;
using NetTopologySuite.Geometries;

namespace Gis.Infrastructure.Entity;

public class PolygonEntity
{
    public Guid Id { get; set; }
    public Polygon Polygon { get; private set; }
    public string Name { get;private set;  }
    public string Desc { get; private set;}
    public string Wkt { get; private set;}
    public int Status { get; private set;}

    public PolygonEntity()
    {
    }

    internal PolygonEntity(Guid id, Polygon polygon, string name, string desc, string wkt, int status)
    {
        Id = id;
        Polygon = polygon;
        Name = name;
        Desc = desc;
        Wkt = wkt;
        Status = status;
    }


    public static PolygonEntity FromAggAndPolygon(PolygonAggregate agg, Polygon poly)
    {
        return new PolygonEntity(agg.Id.Value, poly, agg.Name.Value, agg.Description.Value, agg.Shell.wkt.Value,
            (int)agg.Status);
    }
}