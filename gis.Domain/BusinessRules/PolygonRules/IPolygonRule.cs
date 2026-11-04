using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.PolygonRules;

public interface IPolygonRule
{
    Result Execute();
}