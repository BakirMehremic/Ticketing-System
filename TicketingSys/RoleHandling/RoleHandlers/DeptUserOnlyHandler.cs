using Microsoft.AspNetCore.Authorization;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Redis;
using TicketingSys.RoleHandling.Policies;
using TicketingSys.Settings;

namespace TicketingSys.RoleHandling.RoleHandlers;

public class DeptUserOnlyHandler(ApplicationDbContext context, IRedisService redisService,
    ILogger<DeptUserOnlyHandler> logger) : 
    BaseRoleHandler<DeptUserOnlyRequirement>(context, redisService, logger)
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DeptUserOnlyRequirement requirement)
    {
        string? userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            ?.Value;

        if (string.IsNullOrEmpty(userId)) return;
        
        UserAccessCacheModel? access = await RedisService.GetOrFetchAccess(userId);
        
        if (access == null) return;

        // use this if you want dept users unable to have admin roles

        //if (!access.IsAdmin && access.HasDepartmentAccess)
        //    context.Succeed(requirement);

        if (access.HasDepartmentAccess)
            context.Succeed(requirement);
        
    }
}