using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
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

    public void ChangePolygonName(string name)
    {
        Name = name;
    }

    public void ChangePolygonShell(List<CoordinateValueObject> shell)
    {
        Polygon = CoordinateFactory.CoordinatesToPolygon(shell);
    }

    public void ChangePolygonDesc(string descriptionValue)
    {
        Desc = descriptionValue;
    }

    public void ChangePolygonStatus(int entityStatus)
    {
        Status = entityStatus;
    }
}