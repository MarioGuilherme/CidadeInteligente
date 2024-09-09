namespace CidadeInteligente.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable {
    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
}