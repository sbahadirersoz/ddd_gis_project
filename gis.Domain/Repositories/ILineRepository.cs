using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;

namespace gis.Domain.Repositories;

public interface ILineRepository:IRepository<LineAggregate,LineID>,ILineDistanceCalculatorContract
{
    Task<LineAggregate?>FindByIdAsync(LineID id,bool tracking = true,  CancellationToken cancellationToken = default);
    Task<bool> IsLineNameExistsAsync(LineName pointName, CancellationToken cancellationToken = default);
    public Task UpdateAsync(LineAggregate entity ,CancellationToken cancellationToken = default);
    
    Task<LineAggregate> FindByNameAsync(LineName name, bool tracking = true, CancellationToken cancellationToken = default);

}