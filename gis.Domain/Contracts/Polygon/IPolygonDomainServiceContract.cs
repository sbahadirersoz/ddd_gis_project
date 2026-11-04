using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface IPolygonDomainServiceContract
{
    Task<Result<PolygonAggregate>> CreatePolygonAggregate(PolygonShell shell, PolygonName polygonName, PolygonDescription? polygonDescription);
    Task<Result<PolygonAggregate>> UpdatePolygonAggregate(PolygonAggregate poly ,  PolygonName? polygonName, PolygonDescription? polygonDescription,PolygonStatus? status , List<CoordinateValueObject> newHoles);
    Task<Result<PolygonAggregate>> AddHoleToPolygon(PolygonAggregate poly ,List<CoordinateValueObject> newHole);
    Result SoftDelete(PolygonAggregate agg);
    

}
