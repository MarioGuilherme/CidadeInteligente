using Microsoft.EntityFrameworkCore.Storage;

namespace CidadeInteligente.Infrastructure.Persistence;
public class UnitOfWork(CidadeInteligenteDbContext dbContext) : IUnitOfWork {
    private readonly CidadeInteligenteDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public Task<int> CompleteAsync() => this._dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync() => this._transaction = await this._dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync() {
        try {
            await this._transaction!.CommitAsync();
        } catch (Exception) {
            await this._transaction!.RollbackAsync();
            throw;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing)
            this._dbContext.Dispose();
    }
}