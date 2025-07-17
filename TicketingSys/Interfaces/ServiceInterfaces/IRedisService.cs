using TicketingSys.Redis;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface IRedisService
{
    Task<UserAccessCacheModel?> GetOrFetchAccess(string userId);
}