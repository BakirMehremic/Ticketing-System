namespace TicketingSys.Redis;

public interface IUserAccessCacheService
{
    Task<UserAccessCacheModel?> GetUserAccessAsync(string userId);

    Task SetUserAccessAsync(string userId, bool isAdmin, bool hasDepartmentAccess);

    Task InvalidateUserAccessAsync(string userId);
}