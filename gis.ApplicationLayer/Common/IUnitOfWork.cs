using gis.Domain.Common;
using gis.Domain.Repositories;

namespace gis.ApplicationLayer.Common;

public interface IUnitOfWork:IDisposable,IAsyncDisposable
{
 public IPointRepository PointRepository { get; }
 Task<int> SaveChangesAsync(CancellationToken cancellationToken);
 int SaveChanges();
 Task BeginTransactionAsync(CancellationToken cancellationToken);
 Task CommitTransactionAsync(CancellationToken cancellationToken);
 Task RollbackTransactionAsync(CancellationToken cancellationToken);
 
 public void Save();
}