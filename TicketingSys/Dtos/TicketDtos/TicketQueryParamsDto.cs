using TicketingSys.Enums;

namespace TicketingSys.Dtos.TicketDtos;

public record TicketQueryParamsDto
{
    public TicketStatusEnum? Status { get; init; }
    public TicketUrgencyEnum? Urgency { get; init; }
    public string? CategoryName { get; init; }
    public string? DepartmentName { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public string? Search { get; init; }
    public bool? IsAssigned { get; init; }
    public bool? HasResponses { get; init; }
}