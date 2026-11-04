using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PolygonRules;

public class PolygonMustBeClosed:IPolygonRule
{
    
    private readonly List<CoordinateValueObject> coordinates;

    public PolygonMustBeClosed(List<CoordinateValueObject> coordinates)
    {
        this.coordinates = coordinates;
    }

    public Result Execute()
        => (coordinates.First().Equals(coordinates.Last()))
            ? Result.Success()
            : Result.Failure(BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED);
}