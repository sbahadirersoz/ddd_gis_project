using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface ITopologySuitePolygonValidationContract
{
    Result IsHoleInsideTheShell(List<CoordinateValueObject> hole, PolygonShell shell);
}