using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Models;
using TicketingSys.Redis;
using TicketingSys.Settings;

namespace TicketingSys.Middleware;

public class RefreshRedisOn403(ILogger<RefreshRedisOn403> logger) : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
    private const string RedisRefreshed = "Retry.RedisRefreshed";

    // before sending 403 query the postgres db and write that data to redis and retry request
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded &&
            context.User.Identity?.IsAuthenticated == true &&
            !context.Items.ContainsKey(RedisRefreshed))
        {
            ICurrentUserService userUtils = context.RequestServices.GetRequiredService<ICurrentUserService>();
            string userId = userUtils.GetUserIdOr401();

            IUserAccessCacheService cacheService =
                context.RequestServices.GetRequiredService<IUserAccessCacheService>();
            ApplicationDbContext dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

            User? user = await dbContext.Users
                .Include(u => u.DepartmentAccesses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null) 
                await cacheService.SetUserAccessAsync(userId, user.IsAdmin, user.DepartmentAccesses.Any());
    
            context.Items[RedisRefreshed] = true;

            IAuthorizationService authService = context.RequestServices.GetRequiredService<IAuthorizationService>();
            AuthorizationResult authResult = await authService.AuthorizeAsync(context.User, null, policy);

            if (authResult.Succeeded)
            {
                await next(context);
                return;
            }
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}