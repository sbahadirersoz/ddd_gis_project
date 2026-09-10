using gis.Domain.Aggregates;
using gis.Domain.ResultPattern;

namespace gis.Domain.BusinessRules.LineRules;

public interface ILineRule
{
    Result Execute();
}