using Microsoft.AspNetCore.Authorization;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Settings;

namespace TicketingSys.RoleHandling.RoleHandlers;

public abstract class BaseRoleHandler<TRequirement>(
    ApplicationDbContext context,
    IRedisService redisService,
    ILogger logger)
    :
        AuthorizationHandler<TRequirement>
    where TRequirement : IAuthorizationRequirement
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly ILogger Logger = logger;
    protected readonly IRedisService RedisService = redisService;
}