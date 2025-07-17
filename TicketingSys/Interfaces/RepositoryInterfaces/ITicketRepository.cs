using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface ITicketRepository : IBaseGenericRepository<Ticket>
{
    Task<List<Ticket>> GetAllTicketByUserId(string id, bool includeNavProperties = true);

    Task<List<Ticket>> GetAllTicketsByDepartmentIds(List<int> ids, bool includeNavProperties = true);

    Task ChangeTicketStatus(TicketStatusEnum status, int ticketId);

    Task<Ticket?> GetTicketBySubmittedByIdAndTicketId(string userId, int ticketId);

    Task<List<Ticket>> GetAllWithProperties();
}
