using System.Linq.Expressions;

namespace CidadeInteligente.Domain.Specifications;

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
