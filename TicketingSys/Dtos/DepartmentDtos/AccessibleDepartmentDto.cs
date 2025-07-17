namespace TicketingSys.Dtos.DepartmentDtos;

public record AccessibleDepartmentDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}