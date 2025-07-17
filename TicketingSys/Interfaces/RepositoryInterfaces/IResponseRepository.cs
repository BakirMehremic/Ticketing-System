using TicketingSys.Models;

namespace TicketingSys.Interfaces.RepositoryInterfaces;

public interface IResponseRepository : IBaseGenericRepository<Response>
{
    Task<List<Response>> GetResponsesByUserId(string userId, bool includeNavProperties = true);

    Task<Response?> GetSentResponseByUserIdAndResponseId(string id, int responseId);

    Task<Response?> GetResponseBySubmittedByAndResponseId(string userId,int responseId, bool includeNavProperties = true);
}
