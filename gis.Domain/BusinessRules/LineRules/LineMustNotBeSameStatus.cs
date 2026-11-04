using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.LineRules;

public class LineMustNotBeSameStatus:ILineRule
{
    
    private readonly LineStatus _status;
    private readonly     LineAggregate _aggregate;
    public LineMustNotBeSameStatus(LineStatus status, LineAggregate aggregate)
    {
        _status = status;
        _aggregate = aggregate;
    }

    public Result Execute()
        => (_aggregate.LineStatus  == _status) ? Result.Failure(BusinessRuleErrors.ENTITY_STATUS_ALREADY_GIVEN_PARAMETER) : Result.Success(); 
}