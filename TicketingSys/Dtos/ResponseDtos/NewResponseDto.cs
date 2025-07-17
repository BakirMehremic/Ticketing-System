using System.ComponentModel.DataAnnotations;
using TicketingSys.Dtos.AttachmentDtos;
using TicketingSys.Enums;

namespace TicketingSys.Dtos.ResponseDtos;

public record NewResponseDto
{
    [Required] public int TicketId { get; init; }

    public List<NewResponseAttachmentDto> Attachments { get; init; } = new();

    [Required] public string Message { get; init; }

    [Required] public TicketStatusEnum Status { get; init; }
}