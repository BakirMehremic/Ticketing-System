namespace TicketingSys.Dtos.CategoryDtos;

public record ViewTicketCategoryDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public required int DepartmentId { get; init; }

}