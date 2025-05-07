using System.Linq.Expressions;

namespace Package.Shared.Extensions;

public static class LinqExtensions
{
    public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
    {
        return source
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize);
    }
    
    public static IOrderedQueryable<TSource> OrderByAscOrDesc<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource,TKey>> keySelector,
        bool asc = true
    )
    {
        return asc ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }
    
    public static IOrderedQueryable<TSource> ThenByAscOrDesc<TSource, TKey>(
        this IOrderedQueryable<TSource> source,
        Expression<Func<TSource,TKey>> keySelector,
        bool asc = true
    )
    {
        return asc ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
    }
}