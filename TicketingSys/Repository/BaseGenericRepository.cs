using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public class BaseGenericRepository<T>(ApplicationDbContext context)
    : IBaseGenericRepository<T> where T : class
{
    protected readonly DbSet<T> dbSet = context.Set<T>();


    // assumes every PK is int Id
    public virtual async Task<T?> 
        GetById(int id, params Expression<Func<T, object>>[] includeProperties)
    {
        if (includeProperties.Length == 0)
            return await dbSet.FindAsync(id);

        IQueryable<T> query = dbSet.AsQueryable();

        // add every lambda function include to query same as .Include(x=>x.Attachments)
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
    }


    public async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includeProperties)
    {
        if(includeProperties.Length==0)
            return await dbSet.ToListAsync();


        IQueryable<T> query = dbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }


    public IQueryable<T> GetQueryable()
    {
        return dbSet.AsQueryable();
    }

    public async Task<T> Add(T entity)
    {
        var entry = await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task Delete(T entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task<bool> DeleteById(int id)
    {
        T? toDelete = await this.GetById(id);

        if (toDelete == null) return false;

        await this.Delete(toDelete);
        return true;
    }

    public async Task Update(T entity)
    {
        dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteList(List<T> entityList)
    {
        dbSet.RemoveRange(entityList);
        await context.SaveChangesAsync();
    }

    public async Task AddList(List<T> entityList)
    {
        await dbSet.AddRangeAsync(entityList);
        await context.SaveChangesAsync();
    }
}