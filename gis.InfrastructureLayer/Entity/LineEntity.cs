using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.InfrastructureLayer.HelperMethods;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.Entity;

public class LineEntity
{
    public Guid Id { get; set; }
    public LineString LineString { get; private set; }
    public string Name { get;private set;  }
    public string Desc { get; private set;}
    public string Wkt { get; private set;}
    public int Status { get; private set;}

    public LineEntity()
    {
    }
    public LineEntity(LineString lineString, string name, string desc, string wkt, int status)
    {
        LineString = lineString;
        Name = name;
        Desc = desc;
        Wkt = wkt;
        Status = status;
    }

    /// <summary>
    /// Burayi düzelt
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static LineEntity ToEntity(LineAggregate domain)
    {
        
        var coordList = CoordinateFactory.FromCoordListToCoordinates(domain.Coordinates);
        var coordToLineString = CoordinateFactory.CoordinatesToLineString(coordList);
        var name = domain.LineName.Value;
        var desc = domain.LineDescription.Value;
        var WKT = domain.WKT.Value;
        var status = domain.LineStatus;
        return new LineEntity(domain.Id.Value, coordToLineString, name, desc, WKT, (int)status);
    }

    internal LineEntity(Guid id, LineString lineString, string name, string desc, string wkt, int status)
    {
        Id = id;
        LineString = lineString;
        Name = name;
        Desc = desc;
        Wkt = wkt;
        Status = status;
    }

    internal void ChangeLineName(string name)
    {
        Name = name;
    }
    internal void ChangeLineDesc(string desc)
    {
        Desc = desc;
    }
    internal void ChangeLineStatus(int status)
    {
        Status = status;
    }
    
    internal void ChangeLineString(List<CoordinateValueObject> coordinates)
    {
        var wkt = CommonGeometryHelpers.FromCoordinateVOListToLineStringWKT(coordinates);
        var coords = CoordinateFactory.FromCoordListToCoordinates(coordinates);
        var linestring = CoordinateFactory.CoordinatesToLineString(coords);
        LineString = linestring;
        Wkt = wkt;
    }
    
    
}