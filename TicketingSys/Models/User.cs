using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace TicketingSys.Models;
public class User : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public bool IsAdmin { get; set; } = false;

    public List<UserDepartmentAccess> DepartmentAccesses { get; set; } = new();
}