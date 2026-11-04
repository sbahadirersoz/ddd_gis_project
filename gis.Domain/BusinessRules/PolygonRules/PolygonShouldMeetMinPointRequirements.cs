using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.PolygonRules;

public class PolygonShouldMeetMinPointRequirements:IPolygonRule
{

    private readonly List<CoordinateValueObject> coords ;
    

    public PolygonShouldMeetMinPointRequirements(List<CoordinateValueObject> coords)
    {
        this.coords = coords;
    }

    public Result Execute() =>
        (coords.Count <= 3)
            ? Result.Failure(BusinessRuleErrors.PolygonRules.INSUFFICENT_POINTS)
            : Result.Success();

        
}