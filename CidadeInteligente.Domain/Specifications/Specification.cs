using System.Linq.Expressions;

namespace CidadeInteligente.Domain.Specifications;

public abstract class Specification<T> where T : class
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public List<string> IncludeStrings { get; } = [];
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    public bool IsSplitQuery { get; private set; }
    public bool IsTrackingEnabled { get; private set; } = true;

    internal void SetCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
    }

    internal void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        ArgumentNullException.ThrowIfNull(includeExpression);
        Includes.Add(includeExpression);
    }

    internal void AddInclude(string includeString)
    {
        if (string.IsNullOrWhiteSpace(includeString))
            throw new ArgumentException("Include string cannot be empty", nameof(includeString));
        IncludeStrings.Add(includeString);
    }

    internal void ApplyPaging(int page, int pageSize)
    {
        if (page < 1)
            throw new ArgumentException("Page must be greater than 0", nameof(page));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

        Page = page;
        PageSize = pageSize;
        Skip = (page - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }

    internal void SetOrderBy(Expression<Func<T, object>> orderBy, bool descending = false)
    {
        ArgumentNullException.ThrowIfNull(orderBy);

        if (descending)
            OrderByDescending = orderBy;
        else
            OrderBy = orderBy;
    }

    internal void DisableTracking() => IsTrackingEnabled = false;

    internal void EnableSplitQuery() => IsSplitQuery = true;
}

public sealed class Specification<T, TResult> where T : class
{
    public Specification<T> Query { get; }
    public Expression<Func<T, TResult?>> Selector { get; }

    internal Specification(Specification<T> query, Expression<Func<T, TResult?>> selector)
    {
        Query = query ?? throw new ArgumentNullException(nameof(query));
        Selector = selector ?? throw new ArgumentNullException(nameof(selector));
    }
}
