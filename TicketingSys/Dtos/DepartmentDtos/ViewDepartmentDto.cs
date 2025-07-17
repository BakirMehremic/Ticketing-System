namespace TicketingSys.Dtos.DepartmentDtos;

public record ViewDepartmentDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}