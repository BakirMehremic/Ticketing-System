namespace TicketingSys.Dtos.TicketDtos;

public record SharedTicketQueryParamsDto : TicketQueryParamsDto
{
    public string? AssignedToId { get; init; }
}