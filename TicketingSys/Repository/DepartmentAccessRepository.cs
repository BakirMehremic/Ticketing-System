using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public sealed class DepartmentAccessRepository(ApplicationDbContext context) : BaseGenericRepository
    <UserDepartmentAccess>(context), IDepartmentAccessRepository
{
    public async Task DeleteDepartmentAccessesByUserId(string id)
    {
        List<UserDepartmentAccess> toDelete = await context.
            UserDepartmentAccess.Where(x => x.UserId == id).ToListAsync();

        if (toDelete.Any())
        {
            context.UserDepartmentAccess.RemoveRange(toDelete);
            await context.SaveChangesAsync();
        }
    }
}
