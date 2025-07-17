using System.Linq.Expressions;
using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface IBaseGenericRepository<T> where T: class
{
    Task<T?> GetById(int id, params Expression<Func<T, object>>[] includeProperties);

    Task<List<T>> GetAll(params Expression<Func<T, object>>[] includeProperties);

    IQueryable<T> GetQueryable();

    Task<T> Add(T entity);

    Task Delete(T entity);

    Task<bool> DeleteById(int id);

    Task DeleteList(List<T> entityList);

    Task AddList(List<T> entityList);

    Task Update(T entity);

}
