using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public sealed class DepartmentRepository(ApplicationDbContext context): 
    BaseGenericRepository<Department>(context), IDepartmentRepository
{
    public async Task<Department?> GetDepartmentByName(string name)
    {
        return await context.Departments.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool> CheckIfDepartmentIsReferenced(int id)
    {
        bool isReferencedByTickets = await context.Tickets.AnyAsync(x => x.DepartmentId == id);
        bool isReferencedByCategories = await context.TicketCategories.
            AnyAsync(x => x.DepartmentId == id);

        if (isReferencedByCategories || isReferencedByTickets) return false;
        return true;
    }

}
