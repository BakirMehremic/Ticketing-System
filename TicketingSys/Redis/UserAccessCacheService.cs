using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TicketingSys.Redis;

public class UserAccessCacheService(IDistributedCache cache, ILogger<UserAccessCacheModel> logger)
    : IUserAccessCacheService
{
    public async Task<UserAccessCacheModel?> GetUserAccessAsync(string userId)
    {
        string? data = await cache.GetStringAsync($"user-access:{userId}");

        if (string.IsNullOrEmpty(data)) return null;

        UserAccessCacheModel? dataSerialized = JsonSerializer.Deserialize<UserAccessCacheModel>(data);

        return dataSerialized;
    }

    public async Task SetUserAccessAsync(string userId, bool isAdmin, bool hasDepartmentAccess)
    {
        UserAccessCacheModel data = new (isAdmin, hasDepartmentAccess);

        await cache.SetStringAsync($"user-access:{userId}",
            JsonSerializer.Serialize(data),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });
    }

    public async Task InvalidateUserAccessAsync(string userId)
    {
        await cache.RemoveAsync($"user-access:{userId}");
    }
}