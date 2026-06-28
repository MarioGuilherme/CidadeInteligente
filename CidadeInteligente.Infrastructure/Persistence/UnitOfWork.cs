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

    public async Task BeginTransactionAsync(CancellationToken cancellationToken) => _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);
            await _transaction!.CommitAsync(cancellationToken);
            return rowsAffected;
        }
        catch (Exception)
        {
            await _transaction!.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _dbContext.Dispose();
    }
}
