using System.Linq.Expressions;

namespace TicketingSys.QueryBuilders;

public abstract class QueryBuilderBase<T>
{
    private readonly List<Expression<Func<T, bool>>> _filters = new();

    protected void AddFilter(Expression<Func<T, bool>> filter)
    {
        _filters.Add(filter);
    }

    public IQueryable<T> BuildQueryable(IQueryable<T> queryable)
    {
        foreach (var filter in _filters)
        {
            queryable = queryable.Where(filter);
        }
        return queryable;
    }
}
