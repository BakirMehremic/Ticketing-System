using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface IDepartmentAccessRepository : IBaseGenericRepository<UserDepartmentAccess>
{
    Task DeleteDepartmentAccessesByUserId(string id);
}
