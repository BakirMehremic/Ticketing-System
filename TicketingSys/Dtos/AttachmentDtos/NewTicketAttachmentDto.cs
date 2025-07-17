using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.AttachmentDtos;

public record NewTicketAttachmentDto([Required] string Path, string Filename, string ContentType);