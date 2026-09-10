using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.PolygonRules.Holes;

public class IsHolesInsideThePoly:IPolygonRule
{

    private readonly ITopologySuitePolygonValidationContract _contract;
    private readonly List<CoordinateValueObject> holes;
    private readonly PolygonAggregate aggregate;

    public IsHolesInsideThePoly(List<CoordinateValueObject> holes, PolygonAggregate aggregate, ITopologySuitePolygonValidationContract contract)
    {
        this.holes = holes;
        this.aggregate = aggregate;
        _contract = contract;
    }

    public Result Execute()
    {
        var isHoleInsideTheShell = _contract.IsHoleInsideTheShell(holes, aggregate.Shell);
        return isHoleInsideTheShell;
        
        }
    }