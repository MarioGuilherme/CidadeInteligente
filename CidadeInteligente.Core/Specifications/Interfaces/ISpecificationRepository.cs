using CidadeInteligente.Core.Common;

namespace CidadeInteligente.Core.Specifications.Interfaces;

public interface ISpecificationRepository<T> where T : class
{
    //Task<T?> GetBySpecAsync(Specification<T> spec);
    //Task<T?> GetProjectionBySpecAsync(Specification<T> spec);
    //Task<IEnumerable<T>> GetAllProjectionsBySpecAsync(Specification<T> spec);
    //Task<PagedResult<T>> GetPagedProjectionAsync(Specification<T> spec);
    //Task<int> CountBySpecAsync(Specification<T> spec);
    //Task<bool> AnyBySpecAsync(Specification<T> spec);


    Task<T?> GetBySpecAsync(Specification<T> spec);
    Task<IEnumerable<T>> GetAllBySpecAsync(Specification<T> spec);
    Task<PagedResult<T>> GetPagedBySpecAsync(Specification<T> spec);

    Task<TResult?> GetBySpecAsync<TResult>(Specification<T, TResult> spec);
    Task<IEnumerable<TResult>> GetAllBySpecAsync<TResult>(Specification<T, TResult> spec);
    Task<PagedResult<TResult>> GetPagedBySpecAsync<TResult>(Specification<T, TResult> spec);

    Task<int> CountBySpecAsync(Specification<T> spec);
    Task<bool> AnyBySpecAsync(Specification<T> spec);

    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
