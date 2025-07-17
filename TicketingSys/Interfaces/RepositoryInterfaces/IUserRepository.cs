using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface IUserRepository : IBaseGenericRepository<User>
{
    // only User models have string ids
    Task<bool> UserHasDepartmentAccess(string userId, int departmentId);

    Task<List<UserDepartmentAccess>> GetAllUserDepartmentAccess(bool includeDepartments = true);

    Task<List<UserDepartmentAccess>> GetUserDepartmentAccessListById(string userId);

    Task<User?> GetUserById(string userId, bool includeDepartmentAccessList = false);

}