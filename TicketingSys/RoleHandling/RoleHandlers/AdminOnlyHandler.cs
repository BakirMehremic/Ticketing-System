using Microsoft.AspNetCore.Authorization;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Redis;
using TicketingSys.RoleHandling.Policies;
using TicketingSys.Settings;

namespace TicketingSys.RoleHandling.RoleHandlers;

public class AdminOnlyHandler(
    ApplicationDbContext context,
    IRedisService redisService,
    ILogger<AdminOnlyHandler> logger)
    : BaseRoleHandler<AdminOnlyRequirement>(context, redisService, logger)
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AdminOnlyRequirement requirement)
    {
        string? userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            ?.Value;

        if (string.IsNullOrEmpty(userId)) return;

        UserAccessCacheModel? access = await RedisService.GetOrFetchAccess(userId);
        
        if (access == null) return;
        
        if (access.IsAdmin) 
            context.Succeed(requirement);
    }
}