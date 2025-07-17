using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.AttachmentDtos;

public record NewResponseAttachmentDto([Required] string Path, string Filename, string ContentType);