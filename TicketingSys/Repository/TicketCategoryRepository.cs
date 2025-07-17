using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public sealed class TicketCategoryRepository(ApplicationDbContext context) :
    BaseGenericRepository<TicketCategory>(context), ITicketCategoryRepository
{
    public async Task<TicketCategory?> GetByNameAndDeptId(string name, int id)
    {
        return await context.TicketCategories.
            FirstOrDefaultAsync(x => x.Name == name && x.DepartmentId == id);
    }

    public async Task<bool> IsCategoryReferencedByTickets(int id)
    {
        return await context.Tickets.AnyAsync(x => x.CategoryId == id);
    }


    public async Task<List<TicketCategory>> GetAllCategoriesFromDepartment(int id)
    {
        return await context.TicketCategories
            .Where(x => x.DepartmentId == id).ToListAsync();
    }

}
