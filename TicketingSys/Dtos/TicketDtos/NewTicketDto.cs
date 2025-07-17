using System.ComponentModel.DataAnnotations;
using TicketingSys.Dtos.AttachmentDtos;
using TicketingSys.Enums;

namespace TicketingSys.Dtos.TicketDtos;

public record NewTicketDto
{
    [Required] public int DepartmentId { get; init; }

    [Required] public int CategoryId { get; init; }

    public List<NewTicketAttachmentDto> Attachments { get; init; } = new();

    [Required] public string Title { get; init; }

    [Required] public TicketUrgencyEnum Urgency { get; init; }

    [Required] public  string Description { get; init; }
}