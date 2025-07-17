using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public sealed class UserRepository(ApplicationDbContext context): 
    BaseGenericRepository<User>(context), IUserRepository
{

    public async Task<bool> UserHasDepartmentAccess(string userId, int departmentId)
    {
        return await context.UserDepartmentAccess.
            AnyAsync(x => x.DepartmentId == departmentId && x.UserId == userId);
    }

    public async Task<List<UserDepartmentAccess>> GetAllUserDepartmentAccess(bool includeDepartments = true)
    {
        if (includeDepartments)
        {
            return await context.UserDepartmentAccess
                .Include(x => x.Department).ToListAsync();
        }
        else
        {
            return await context.UserDepartmentAccess.ToListAsync();
        }
    }

    public async Task<List<UserDepartmentAccess>> GetUserDepartmentAccessListById(string userId)
    {
        return await context.UserDepartmentAccess
            .Include(x => x.Department)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<User?> GetUserById(string userId, bool includeDeptAccessList=false)
    {
        if (!includeDeptAccessList)
            return await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        return await context.Users.Include(x => x.DepartmentAccesses)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
