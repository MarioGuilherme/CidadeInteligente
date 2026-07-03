using CidadeInteligente.Domain.Repositories;

namespace CidadeInteligente.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IAreaRepository Areas { get; }
    ICourseRepository Courses { get; }
    IProjectRepository Projects { get; }
    IUserRepository Users { get; }

    Task<int> ExecuteInTransactionAsync(
        Action operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteInTransactionAsync(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default);

    Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default);

    //Task ExecuteInTransactionAsync(
    //    Func<CancellationToken, Task> operation,
    //    Func<CancellationToken, Task>? onRollback = null,
    //    CancellationToken cancellationToken = default);

    //Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    //Task<int> CommitAsync(CancellationToken cancellationToken);
    //Task RollbackAsync(CancellationToken cancellationToken = default);
}
