using TicketingSys.Dtos.ResponseDtos;

namespace TicketingSys.Dtos.TicketDtos;

public record ViewTicketDto
{
    public required int Id { get; init; }

    public required string Title { get; init; }
    public required string Description { get; init; }

    public required string Status { get; init; }
    public required string Urgency { get; init; }

    public required string SubmittedById { get; init; }
    public required string SubmittedByName { get; init; }

    public string? AssignedToId { get; init; }
    public string? AssignedToName { get; init; }

    public required string DepartmentName { get; init; }
    public required string CategoryName { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    public required List<string> AttachmentPaths { get; init; }

    public List<ViewResponseDto>? ViewResponses { get; set; }

    public int? ResponsesCount { get; set; }
}