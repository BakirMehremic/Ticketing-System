using Microsoft.AspNetCore.Authorization;

namespace TicketingSys.RoleHandling.Policies;

public class AdminOnlyRequirement : IAuthorizationRequirement
{
}