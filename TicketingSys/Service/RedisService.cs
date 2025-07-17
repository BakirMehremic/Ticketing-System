using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Models;
using TicketingSys.Redis;
using TicketingSys.Settings;

namespace TicketingSys.Service;

public class RedisService(
    IUserAccessCacheService cache,
    IUserRepository userRepository) : IRedisService
{
    public async Task<UserAccessCacheModel?> GetOrFetchAccess(string userId)
    {
        UserAccessCacheModel? access = await cache.GetUserAccessAsync(userId);
        if (access != null) return access;
        
        User? user = await userRepository.GetUserById(userId, includeDepartmentAccessList: true);
        if (user == null) return null;

        access = new UserAccessCacheModel(user.IsAdmin, user.DepartmentAccesses.Any());

        await cache.SetUserAccessAsync(userId, access.IsAdmin, access.HasDepartmentAccess);
        return access;
    }
}