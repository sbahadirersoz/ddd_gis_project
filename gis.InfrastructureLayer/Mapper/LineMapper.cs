using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern.Errors;
using gis.InfrastructureLayer.Entity;
using gis.InfrastructureLayer.HelperMethods;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.Mapper;

public class LineMapper:IMapperContract<LineAggregate,LineEntity>
{
    public static LineAggregate ToDomain(LineEntity dbEntity)
    {
        var id = LineID.FromGuid(dbEntity.Id).Value;
        var name = LineName.FromString(dbEntity.Name).Value;
        var dbEntityToCoordVo = DbEntityToCoordVO(dbEntity.LineString);
        var desc = LineDescription.FromString(dbEntity.Desc).Value;
        var status = (LineStatus)dbEntity.Status;
        var wkt  = WellKnownText.Create(dbEntity.Wkt).Value;
        return LineAggregate.Reconstitute(id, name, dbEntityToCoordVo, desc,wkt,status);
    }

    public static LineEntity ToDbEntity(LineAggregate domain)
    {
        return LineEntity.ToEntity(domain);
    }

    private static List<CoordinateValueObject> DbEntityToCoordVO(LineString line)
    {
        return line.Coordinates.Select(x =>
            {
                var Lon = Longitude.Create(x.X).Value;
                var Lat = Latitude.Create(x.X).Value;
                return CoordinateValueObject.ForLineCoordCreation(Lat, Lon).Value;
            }
        ).ToList
            ();
    }
    
}