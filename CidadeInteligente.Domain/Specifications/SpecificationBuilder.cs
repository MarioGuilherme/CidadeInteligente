using System.Linq.Expressions;

namespace CidadeInteligente.Domain.Specifications;

public class SpecificationBuilder<T> where T : class
{
    private readonly Specification<T> _spec;

    private SpecificationBuilder(Specification<T> spec)
    {
        _spec = spec ?? throw new ArgumentNullException(nameof(spec));
    }

    public static SpecificationBuilder<T> Create() => new(new BaseSpecification<T>());

    public SpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
    {
        _spec.SetCriteria(criteria);
        return this;
    }

    public SpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
    {
        _spec.AddInclude(includeExpression);
        return this;
    }

    public SpecificationBuilder<T> Include(string includeString)
    {
        _spec.AddInclude(includeString);
        return this;
    }

    public SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderBy)
    {
        _spec.SetOrderBy(orderBy, descending: false);
        return this;
    }

    public SpecificationBuilder<T> OrderByDesc(Expression<Func<T, object>> orderBy)
    {
        _spec.SetOrderBy(orderBy, descending: true);
        return this;
    }

    public SpecificationBuilder<T> PageBy(int page, int pageSize)
    {
        _spec.ApplyPaging(page, pageSize);
        return this;
    }

    public SpecificationBuilder<T> NoTracking()
    {
        _spec.DisableTracking();
        return this;
    }

    public SpecificationBuilder<T> SplitQuery()
    {
        _spec.EnableSplitQuery();
        return this;
    }

    public Specification<T> Build()
    {
        return _spec;
    }

    public Specification<T, TResult?> WithProjection<TResult>(Expression<Func<T, TResult?>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _spec.DisableTracking();
        return new(_spec, selector);
    }
}
