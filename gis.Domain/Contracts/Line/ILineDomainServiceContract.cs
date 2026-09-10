using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface ILineDomainServiceContract
{
    Task<Result<LineAggregate>> CreateLineAggregate(List<CoordinateValueObject> coordinates, LineName lineName, LineDescription? lineDescription);
    Task<Result<LineAggregate>> UpdateLineAggregate(LineAggregate agg,List<CoordinateValueObject> coordinates, LineName? lineName, LineDescription? lineDescription,LineStatus? status);
    Result SoftDelete(LineAggregate agg);
}