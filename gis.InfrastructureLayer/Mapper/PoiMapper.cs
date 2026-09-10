using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.InfrastructureLayer.Entity;
using gis.InfrastructureLayer.HelperMethods;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.Mapper;

public class PoiMapper:IMapperContract<POIAggregate,PoiEntity>
{


    
    public static PoiEntity ToDbEntity(POIAggregate domain)
    {
        return PoiEntity.AggToEntity(domain);
    }
    
    public static POIAggregate ToDomain(PoiEntity poi)
    {
        var fromGuid = PointID.FromGuid(poi.Id).Value;
        var coordinateValueObject = EntityPointToCoordinates(poi.Point);
        var fromStringDesc = PointDescription.FromString(poi.Desc).Value;
        var fromStringName = PointName.FromString(poi.Name).Value;
        var poiStatus = (POIStatus)poi.Status;
        return POIAggregate.Reconstitute(fromGuid, coordinateValueObject, fromStringDesc, fromStringName, poiStatus);

    }

    
    public static Point AggregateCoordinatesToEntityPoint(CoordinateValueObject coordinateValueObject) => CoordinateFactory.LonLatToPoint(coordinateValueObject.Longitude, coordinateValueObject.Latitude);
    
    public static CoordinateValueObject EntityPointToAggregateCoordinates(PoiEntity poi)
    {
        return EntityPointToCoordinates(poi.Point);
    }
    public static Point PointAggregateWkttoEntityPoint(WellKnownText wkt)
    {
        return CommonGeometryHelpers.WktToPoint(wkt);
    }


    
    private static CoordinateValueObject EntityPointToCoordinates(Point point)
    {
        var pointCoordinates = point.Coordinates[0];
        var lon = Longitude.Create(pointCoordinates.X).Value;
        var lat = Latitude.Create(pointCoordinates.Y).Value;
        var fromLonLatCoordinatesToWktString = CommonGeometryHelpers.FromLonLatCoordinatesToWKTString(lat,lon);
        var wkt = WellKnownText.Create(fromLonLatCoordinatesToWktString.Value).Value;
        return CoordinateValueObject.FromLonLat(lat, lon, wkt).Value;
    }
    private static PointName EntityNameToAggregateName(string name)=> PointName.FromString(name).Value;
    private static PointDescription EntityDescToAggregateDesc(string value)=> PointDescription.FromString(value).Value;
    private static POIStatus EntityStatusToAggregateStatus(int value)=>(POIStatus)value ;
    
}