using gis.Domain.Common;
using gis.Domain.Repositories;

namespace gis.ApplicationLayer.Common;

public interface IUnitOfWork:IDisposable,IAsyncDisposable
{
 public IPointRepository PointRepository { get; }
 Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
 int SaveChanges();
 Task BeginTransactionAsync(CancellationToken cancellationToken=default);
 Task CommitTransactionAsync(CancellationToken cancellationToken=default);
 Task RollbackTransactionAsync(CancellationToken cancellationToken=default);
 
 public void Save();
}