using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Settings;

namespace TicketingSys.Service;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository repository) : ICurrentUserService
{
    public string GetUserIdOr401()
    {
        ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;
        string? userId = user?.FindFirst("sub")?.Value
                         ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // handling the exception and returning 401 is handled in ExceptionHandlingMiddleware
        if (string.IsNullOrEmpty(userId))
        {
            throw new NoUserIdInJwtException("User not authenticated.");
        }

        return userId;
    }

    public async Task HasDepartmentAccessOr403(int deptId)
    {
        string userId = GetUserIdOr401();

        bool access = await repository.UserHasDepartmentAccess(userId, deptId);

        // middleware handles and returns 403
        if (!access)
        {
            throw new ForbiddenException("You do not have access to this department.");
        }
    }
}