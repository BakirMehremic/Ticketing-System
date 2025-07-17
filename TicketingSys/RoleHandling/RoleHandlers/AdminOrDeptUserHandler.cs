using Microsoft.AspNetCore.Authorization;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Redis;
using TicketingSys.RoleHandling.Policies;
using TicketingSys.Settings;

namespace TicketingSys.RoleHandling.RoleHandlers;

public class AdminOrDeptUserHandler(
    ApplicationDbContext context,
    IRedisService redisService,
    ILogger<AdminOrDeptUserHandler> logger)
    : BaseRoleHandler<AdminOrDeptUserRequirement>(context, redisService, logger)
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AdminOrDeptUserRequirement requirement)
    {
        string? userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            ?.Value;

        if (string.IsNullOrEmpty(userId)) return;

        UserAccessCacheModel? access = await RedisService.GetOrFetchAccess(userId);
        
        if (access == null) return;
        
        if (access.IsAdmin || access.HasDepartmentAccess) 
            context.Succeed(requirement);
    }
}