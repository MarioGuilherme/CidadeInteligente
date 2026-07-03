using CidadeInteligente.Core.Common;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Core.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Repositories;


public abstract class SpecificationRepositoryBase<T>(CidadeInteligenteDbContext context) : ISpecificationRepository<T> where T : class
{
    protected readonly CidadeInteligenteDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetBySpecAsync(Specification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllBySpecAsync(Specification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<PagedResult<T>> GetPagedBySpecAsync(Specification<T> spec)
    {
        int totalCount = await ApplyCriteriaOnly(spec).CountAsync();
        List<T> items = await ApplySpecification(spec).ToListAsync();

        return new PagedResult<T>(items, totalCount, spec.PageNumber, spec.PageSize);
    }

    public async Task<TResult?> GetBySpecAsync<TResult>(Specification<T, TResult> spec)
    {
        return await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TResult>> GetAllBySpecAsync<TResult>(Specification<T, TResult> spec)
    {
        return await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetPagedBySpecAsync<TResult>(Specification<T, TResult> spec)
    {
        const int pageSize = 8;

        Specification<T> countSpec = SpecificationBuilder<T>.Create().Build();
        int totalCount = await CountBySpecAsync(countSpec);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        int page = spec.Query.PageNumber < 1 ? 1 : spec.Query.PageNumber;
        if (totalPages > 0 && page > totalPages)
            page = totalPages;

        List<TResult> items = (await ApplySpecification(spec.Query)
            .Select(spec.Selector)
            .ToListAsync())!;

        return new PagedResult<TResult>(items, totalCount, spec.Query.PageNumber, spec.Query.PageSize);
    }

    public async Task<int> CountBySpecAsync(Specification<T> spec)
    {
        return await ApplyCriteriaOnly(spec).CountAsync();
    }

    public async Task<bool> AnyBySpecAsync(Specification<T> spec)
    {
        return await ApplyCriteriaOnly(spec).AnyAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
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
}
