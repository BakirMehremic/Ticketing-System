using Microsoft.AspNetCore.Authorization;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Redis;
using TicketingSys.RoleHandling.Policies;
using TicketingSys.Settings;

namespace TicketingSys.RoleHandling.RoleHandlers;

public class RegularUserOnlyHandler(ApplicationDbContext context, IRedisService redisService,
    ILogger<RegularUserOnlyHandler> logger) : 
    BaseRoleHandler<RegularUserOnlyRequirement>(context, redisService, logger)
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RegularUserOnlyRequirement requirement)
    {
        string? userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            ?.Value;

        if (string.IsNullOrEmpty(userId)) return;
        
        UserAccessCacheModel? access = await RedisService.GetOrFetchAccess(userId);
        
        if (access == null) return;

        if (!access.IsAdmin && !access.HasDepartmentAccess)
            context.Succeed(requirement);
    }
}