using CidadeInteligente.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CidadeInteligente.Infrastructure.Persistence;

public class UnitOfWork(
    CidadeInteligenteDbContext dbContext,
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

    public Task<int> CompleteAsync() => _dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync() => _transaction = await _dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        try
        {
            await _transaction!.CommitAsync();
        }
        catch (Exception)
        {
            await _transaction!.RollbackAsync();
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