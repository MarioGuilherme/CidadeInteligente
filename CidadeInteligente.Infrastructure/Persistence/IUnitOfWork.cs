using CidadeInteligente.Core.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IAreaRepository Areas { get; }
    ICourseRepository Courses { get; }
    IProjectRepository Projects { get; }
    IUserRepository Users { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
