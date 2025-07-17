using TicketingSys.Dtos.AttachmentDtos;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Service;

public class AttachmentService(IBaseGenericRepository<TicketAttachment> ticketAttachmentRepository,
    IBaseGenericRepository<ResponseAttachment> responseAttachmentRepository) : IAttachmentService
{
    public async Task<List<TicketAttachment>> CreateAndSaveTicketAttachments(int ticketId,
        List<NewTicketAttachmentDto> attachmentsDto)
    {
        List<TicketAttachment> attachments = attachmentsDto.Select(a => new TicketAttachment
        {
            TicketId = ticketId,
            Path = a.Path,
            Filename = a.Filename,
            ContentType = a.ContentType
        }).ToList();

        await ticketAttachmentRepository.AddList(attachments);

        return attachments;
    }

    public async Task<List<ResponseAttachment>> CreateAndSaveResponseAttachments(int responseId,
        List<NewResponseAttachmentDto> dtoList)
    {
        List<ResponseAttachment> attachments = dtoList.Select(a => new ResponseAttachment
        {
            ResponseId = responseId,
            Path = a.Path,
            FileName = a.Filename,
            ContentType = a.ContentType
        }).ToList();

        await responseAttachmentRepository.AddList(attachments);

        return attachments;
    }

}