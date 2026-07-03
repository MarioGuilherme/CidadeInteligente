using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CidadeInteligente.Infrastructure.Persistence;

public class UnitOfWork(CidadeInteligenteDbContext dbContext,
    IAreaRepository areas,
    ICourseRepository courses,
    IProjectRepository projects,
    IUserRepository users) : IUnitOfWork
{
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IAreaRepository Areas { get; } = areas;
    public ICourseRepository Courses { get; } = courses;
    public IProjectRepository Projects { get; } = projects;
    public IUserRepository Users { get; } = users;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("There is no active transaction to confirm.");

        try
        {
            int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            return rowsAffected;
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
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
    }
}
