using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Specifications;
using System.Linq.Expressions;

namespace CidadeInteligente.Domain.Repositories;

public interface ISpecificationRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<bool> AnyBySpecAsync(Specification<T> spec, CancellationToken cancellationToken);

    Task<int> DeleteByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    Task<List<TResult>> GetAllBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken);
    Task<T?> GetBySpecAsync(Specification<T> spec, CancellationToken cancellationToken);
    Task<TResult?> GetBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken);
    Task<PagedResult<TResult>> GetPagedBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken);
}
