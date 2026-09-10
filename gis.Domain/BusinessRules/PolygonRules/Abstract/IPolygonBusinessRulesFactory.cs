using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.PolygonRules.Abstract;

public interface IPolygonBusinessRulesFactory
{
    Result ValidateIsHoleInsideTheShell(List<CoordinateValueObject> list, PolygonAggregate agg);
}