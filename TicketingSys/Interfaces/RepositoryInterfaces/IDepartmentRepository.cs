using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface IDepartmentRepository: IBaseGenericRepository<Department>
{
    Task<Department?> GetDepartmentByName(string name);

    Task<bool> CheckIfDepartmentIsReferenced(int id);

}
