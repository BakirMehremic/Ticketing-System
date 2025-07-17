using TicketingSys.Dtos.AttachmentDtos;
using TicketingSys.Models;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface IAttachmentService
{
    Task<List<TicketAttachment>> CreateAndSaveTicketAttachments(int ticketId,
        List<NewTicketAttachmentDto> attachmentsDto);

    Task<List<ResponseAttachment>> CreateAndSaveResponseAttachments(int responseId,
        List<NewResponseAttachmentDto> dtoList);
}