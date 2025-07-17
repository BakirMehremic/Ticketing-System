using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.Mappers;

public static class TicketMapper
{
    // maps to a model without ticketAttachments because that table needs
    // Ticket Id, they will be mapped to the Ticket in AttachmentMapper
    public static Ticket NewDtoToModel(this NewTicketDto dto, string createdById, int categoryId, int departmentId)
    {
        return new Ticket
        {
            SubmittedById = createdById,
            CategoryId = categoryId,
            DepartmentId = departmentId,
            Status = TicketStatusEnum.Open,
            Title = dto.Title,
            Urgency = dto.Urgency,
            CreatedAt = DateTime.UtcNow,
            Description = dto.Description,
            Attachments = new List<TicketAttachment>()
        };
    }

    public static Ticket NewDtoToModel(this NewTicketDto dto, string createdById){
        return new Ticket
        {
            SubmittedById = createdById,
            CategoryId = dto.CategoryId,
            DepartmentId = dto.DepartmentId,
            Status = TicketStatusEnum.Open,
            Title = dto.Title,
            Urgency = dto.Urgency,
            CreatedAt = DateTime.UtcNow,
            Description = dto.Description,
            Attachments = new List<TicketAttachment>()
        };
    }

    public static ViewTicketDto ModelToViewDto(this Ticket ticket)
    {
        return new ViewTicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            Urgency = ticket.Urgency.ToString(),
            SubmittedById = ticket.SubmittedById,
            SubmittedByName = ticket.SubmittedBy?.FirstName ?? "Unknown",
            AssignedToId = ticket.AssignedToId,
            AssignedToName = ticket.AssignedTo?.FirstName ?? "Unassigned",
            DepartmentName = ticket.Department?.Name ?? "Unknown",
            CategoryName = ticket.Category?.Name ?? "Unknown",
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            AttachmentPaths = ticket.Attachments?.Select(a => a.Path).ToList() ?? new List<string>()
        };
    }

    // this overload adds list of related responses to the ticket dto if the ticket query includes them
    public static ViewTicketDto ModelToViewDto(this Ticket ticket, bool includesResponses)
    {
        ViewTicketDto dto = ticket.ModelToViewDto();

        if (includesResponses && ticket.Responses != null)
        {
            dto.ViewResponses = ticket.Responses
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => r.ToViewDto(false))
                .ToList();

            dto.ResponsesCount = ticket.Responses.Count();
        }

        return dto;
    }

    public static List<ViewTicketDto> ModelToViewDtoList(this List<Ticket> tickets)
    {
        return tickets.Select(t => t.ModelToViewDto()).ToList();
    }
}