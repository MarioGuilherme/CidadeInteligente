using CidadeInteligente.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CidadeInteligente.Infrastructure.Persistence;

public class UnitOfWork(CidadeInteligenteDbContext dbContext,
    IAreaRepository areas,
    ICourseRepository courses,
    IProjectRepository projects,
    IUserRepository users,
    ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IAreaRepository Areas { get; } = areas;
    public ICourseRepository Courses { get; } = courses;
    public IProjectRepository Projects { get; } = projects;
    public IUserRepository Users { get; } = users;

    public Task<int> ExecuteInTransactionAsync(
        Action operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default)
        => ExecuteInTransactionAsync(_ =>
        {
            operation();
            return Task.CompletedTask;
        }, onRollback, cancellationToken);

    public async Task<int> ExecuteInTransactionAsync(
        Func<CancellationToken, Task> operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default)
    {
        (_, int rowsAffected) = await ExecuteCoreAsync<object?>(async ct =>
        {
            await operation(ct);
            return null;
        }, onRollback, cancellationToken);

        return rowsAffected;
    }

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        Func<CancellationToken, Task>? onRollback = default,
        CancellationToken cancellationToken = default)
    {
        (T result, _) = await ExecuteCoreAsync(operation, onRollback, cancellationToken);
        return result;
    }

    private async Task<(T Result, int RowsAffected)> ExecuteCoreAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        Func<CancellationToken, Task>? onRollback,
        CancellationToken cancellationToken)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            T result = await operation(cancellationToken);
            int rowsAffected = await CommitAsync(cancellationToken);
            return (result, rowsAffected);
        }
        catch
        {
            await RollbackAsync(cancellationToken);

            if (onRollback is not null)
            {
                try
                {
                    await onRollback(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Post-rollback compensation failure");
                }
            }

            throw;
        }
    }

    private async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);
        await _transaction!.CommitAsync(cancellationToken);
        await DisposeTransactionAsync();
        return rowsAffected;
    }

    private async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    private async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null) return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is null) return;

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
