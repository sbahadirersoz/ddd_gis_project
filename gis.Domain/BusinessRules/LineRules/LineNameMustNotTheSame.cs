using gis.Domain.Aggregates;
using gis.Domain.Entities.Information.Line;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.BusinessRules.LineRules;

public class LineNameMustNotTheSame:ILineRule
{
    private readonly LineName _name;
    private readonly    LineAggregate _aggregate;
 
    public LineNameMustNotTheSame(LineName name, LineAggregate aggregate)
    {
        _name = name;
        _aggregate = aggregate;
    }

    public Result Execute()
        => string.Equals(_aggregate.LineName.Value, _name.Value) ? Result.Failure(DomainErrors.LineErrors.Name.SAME_VALUE_PROVIDED) : Result.Success();
}