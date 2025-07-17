using TicketingSys.Enums;

namespace TicketingSys.Dtos.ResponseDtos;

public record ResponseQueryParamsDto
{
    public string? Search { get; init; }
    public TicketStatusEnum? Status { get; init; }
    public string? CategoryName { get; init; }
    public string? DepartmentName { get; init; }
    public string? AssignedToName { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public bool? HasAttachments { get; init; }
}