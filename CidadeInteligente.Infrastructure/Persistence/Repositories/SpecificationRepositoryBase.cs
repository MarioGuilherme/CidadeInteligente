using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;

public abstract class SpecificationRepositoryBase<T>(CidadeInteligenteDbContext context) : ISpecificationRepository<T> where T : class
{
    protected readonly CidadeInteligenteDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AnyBySpecAsync(Specification<T> spec, CancellationToken cancellationToken)
    {
        return await ApplyCriteriaOnly(spec).AnyAsync(cancellationToken);
    }

    public async Task<int> DeleteByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await DbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<TResult>> GetAllBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken)
    {
        return (await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .ToListAsync(cancellationToken))!;
    }

    public async Task<T?> GetBySpecAsync(Specification<T> spec, CancellationToken cancellationToken)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken)
    {
        return await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<TResult>> GetPagedBySpecAsync<TResult>(Specification<T, TResult> spec, CancellationToken cancellationToken)
    {
        int totalCount = await ApplyCriteriaOnly(spec.Query).CountAsync(cancellationToken);
        int page = ClampPage(spec.Query.Page, spec.Query.PageSize, totalCount);

        List<TResult> items = (await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .ToListAsync(cancellationToken))!;

        return new(items, totalCount, page, spec.Query.PageSize);
    }

    protected IQueryable<T> ApplySpecification(Specification<T> spec)
    {
        IQueryable<T> query = DbSet.AsQueryable();

        if (!spec.IsTrackingEnabled)
            query = query.AsNoTracking();

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        if (spec.IsSplitQuery)
            query = query.AsSplitQuery();

        query = spec.Includes.Aggregate(query,
            (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query,
            (current, includeName) => current.Include(includeName));

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);

        return query;
    }

    protected IQueryable<T> ApplyCriteriaOnly(Specification<T> spec)
    {
        IQueryable<T> query = DbSet.AsQueryable();

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        return query;
    }

    private static int ClampPage(int requestedPage, int pageSize, int totalCount)
    {
        if (pageSize < 1) return 1;

        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        int page = requestedPage < 1 ? 1 : requestedPage;
        if (totalPages > 0 && page > totalPages)
            page = totalPages;

        return page;
    }
}
