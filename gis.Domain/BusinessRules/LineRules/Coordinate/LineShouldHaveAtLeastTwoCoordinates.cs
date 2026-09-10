using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.LineRules.Coordinate;

public class LineShouldHaveAtLeastTwoCoordinates:ILineRule
{
    
    private readonly List<CoordinateValueObject> _list;

    public LineShouldHaveAtLeastTwoCoordinates(List<CoordinateValueObject> list)
    {
        _list = list;
    }

    public Result Execute() => _list.Count is < 2
        ? Result.Failure(BusinessRuleErrors.LineRules.INSUFFICENT_POINTS)
        : Result.Success();
}