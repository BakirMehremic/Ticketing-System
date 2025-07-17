using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface ITicketCategoryRepository : IBaseGenericRepository<TicketCategory>
{
    Task<TicketCategory?> GetByNameAndDeptId(string name, int id);

    Task<bool> IsCategoryReferencedByTickets(int id);

    Task<List<TicketCategory>> GetAllCategoriesFromDepartment(int id);
}
