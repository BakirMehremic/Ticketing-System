using TicketingSys.Dtos.DepartmentDtos;

namespace TicketingSys.Dtos.UserDtos;

public record ViewUserDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public List<AccessibleDepartmentDto>? AccessibleDepartmentDtos { get; set; }

    public required string UserId { get; set; }

    public bool IsAdmin { get; set; }
}