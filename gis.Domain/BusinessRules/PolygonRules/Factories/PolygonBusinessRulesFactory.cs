using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.PolygonRules.Abstract;
using gis.Domain.BusinessRules.PolygonRules.Holes;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.PolygonRules;

public class PolygonBusinessRulesFactory:IPolygonBusinessRulesFactory
{
    private readonly ITopologySuitePolygonValidationContract contract;

    public PolygonBusinessRulesFactory(ITopologySuitePolygonValidationContract contract)
    {
        this.contract = contract;
    }


public Result ValidateIsHoleInsideTheShell(List<CoordinateValueObject> list,PolygonAggregate agg)
        => CheckRule(new IsHolesInsideThePoly(list,agg,contract));
    private Result CheckRule(IPolygonRule rule) => rule.Execute();
}