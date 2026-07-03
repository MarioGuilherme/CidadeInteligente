using System.Linq.Expressions;

namespace CidadeInteligente.Domain.Specifications;

public abstract class Specification<T> where T : class
{
    //public Expression<Func<T, bool>>? Criteria { get; private set; }
    //public List<Expression<Func<T, object>>> Includes { get; } = [];
    //public List<string> IncludeStrings { get; } = [];
    //public Expression<Func<T, object>>? OrderBy { get; private set; }
    //public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    //public int Skip { get; private set; }
    //public int Take { get; private set; }
    //public int PageNumber { get; private set; }
    //public int PageSize { get; private set; }
    //public bool IsPagingEnabled { get; private set; }
    //public bool IsSplitQuery { get; private set; }
    //public bool IsTrackingEnabled { get; private set; } = true;
    //public Expression<Func<T, T>>? ProjectionSelector { get; private set; }

    //internal void SetCriteria(Expression<Func<T, bool>> criteria)
    //{
    //    ArgumentNullException.ThrowIfNull(criteria);
    //    Criteria = criteria;
    //}

    //internal void AddInclude(Expression<Func<T, object>> includeExpression)
    //{
    //    ArgumentNullException.ThrowIfNull(includeExpression);
    //    Includes.Add(includeExpression);
    //}

    //internal void AddInclude(string includeString)
    //{
    //    if (string.IsNullOrWhiteSpace(includeString))
    //        throw new ArgumentException("Include string não pode ser vazio", nameof(includeString));
    //    IncludeStrings.Add(includeString);
    //}

    //internal void ApplyPaging(int pageNumber, int pageSize)
    //{
    //    if (pageNumber < 1)
    //        throw new ArgumentException("Page number deve ser maior que 0", nameof(pageNumber));
    //    if (pageSize < 1)
    //        throw new ArgumentException("Page size deve ser maior que 0", nameof(pageSize));

    //    PageNumber = pageNumber;
    //    PageSize = pageSize;
    //    Skip = (pageNumber - 1) * pageSize;
    //    Take = pageSize;
    //    IsPagingEnabled = true;
    //}

    //internal void SetOrderBy(Expression<Func<T, object>> orderBy, bool descending = false)
    //{
    //    ArgumentNullException.ThrowIfNull(orderBy);

    //    if (descending)
    //        OrderByDescending = orderBy;
    //    else
    //        OrderBy = orderBy;
    //}

    //internal void DisableTracking()
    //{
    //    IsTrackingEnabled = false;
    //}

    //internal void EnableSplitQuery()
    //{
    //    IsSplitQuery = true;
    //}

    //internal void SetProjection(Expression<Func<T, T>> projectionSelector)
    //{
    //    ArgumentNullException.ThrowIfNull(projectionSelector);
    //    ProjectionSelector = projectionSelector;
    //}


    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public List<string> IncludeStrings { get; } = [];
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public int PageNumber { get; private set; }
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
        if (includeExpression is null)
            throw new ArgumentNullException(nameof(includeExpression));
        Includes.Add(includeExpression);
    }

    internal void AddInclude(string includeString)
    {
        if (string.IsNullOrWhiteSpace(includeString))
            throw new ArgumentException("Include string não pode ser vazio", nameof(includeString));
        IncludeStrings.Add(includeString);
    }

    internal void ApplyPaging(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number deve ser maior que 0", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size deve ser maior que 0", nameof(pageSize));

        PageNumber = pageNumber;
        PageSize = pageSize;
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }

    internal void SetOrderBy(Expression<Func<T, object>> orderBy, bool descending = false)
    {
        if (orderBy is null)
            throw new ArgumentNullException(nameof(orderBy));

        if (descending)
            OrderByDescending = orderBy;
        else
            OrderBy = orderBy;
    }

    internal void DisableTracking() => IsTrackingEnabled = false;

    internal void EnableSplitQuery() => IsSplitQuery = true;
}

public class BaseSpecification<T> : Specification<T> where T : class
{
}
