using TicketingSys.Dtos.TicketDtos;

namespace TicketingSys.Dtos.ResponseDtos;

public record ViewResponseDto
{
    public int Id { get; init; }
    public int TicketId { get; init; }
    public ViewTicketDto? Ticket { get; set; }
    public required string UserId { get; init; }
    public required List<string> AttachmentUrls { get; init; }
    public required string Message { get; init; }
    public required string Status { get; init; } // converted from enum to string in mapper
    public DateTime CreatedAt { get; init; }
}