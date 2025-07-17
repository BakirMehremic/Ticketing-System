namespace TicketingSys.Dtos.UserDtos;

public record UserQueryParamsDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public bool? IsAdmin { get; set; }
    public bool? HasDepartments { get; set; }
}