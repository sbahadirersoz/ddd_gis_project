using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.InfrastructureLayer.HelperMethods;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.Entity;

public class PoiEntity
{
    public Guid Id { get; set; }
    public Point Point { get;private set; }
    public string Name { get;  private set;}
    public string Desc { get; private set;}
    public string Wkt { get; private set;}
    public int Status { get; private set;}
    private PoiEntity()
    {
    }

    private PoiEntity(Guid id, Point point,string name, string desc, string wkt, int status)
    {
        Id = id;
        Name = name;
        Desc = desc;
        Wkt = wkt;
        Point = point;
        Status = status;
    }

    public static PoiEntity AggToEntity(POIAggregate agg)
    {
        var PointFromCoords = new Point(agg.CoordinateValueObject.Latitude.Value,agg.CoordinateValueObject.Longitude.Value);
        var id = agg.Id.Value;
        var pointNameValue = agg.PointName.Value;
        var pointDescValue = agg.PointDesc.Value;
        var wktValue = agg.CoordinateValueObject.WKT.Value;
        var status = (int)agg.Status;
        return new PoiEntity(id, PointFromCoords, pointNameValue,pointDescValue, wktValue, status);
    }

    internal void SoftDelete()
    {
        Status = 2;
    }    internal void ChangeStatus(int status)
    {
        Status = status;
    }internal void ChangeName(string name)
    {
        Name = name;
    }internal void ChangeDesc(string desc)
    {
        Desc = desc;
    }internal void ChangeCoords(Latitude lat ,Longitude lng)
    {
        var coord = CoordinateFactory.FromLonLatToPoint(lat,lng); 
        if (coord.IsFailure) return;
        var fromLonLatCoordinatesToWktString = CommonGeometryHelpers.FromLonLatCoordinatesToWKTString(lat,lng);
        if (fromLonLatCoordinatesToWktString.IsFailure) return;
        Point = coord.Value;
        Wkt = fromLonLatCoordinatesToWktString.Value; 
    }
}