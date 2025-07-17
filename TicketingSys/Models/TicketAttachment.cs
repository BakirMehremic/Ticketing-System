using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSys.Models;

[Table("TicketAttachments")]
public class TicketAttachment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    // path or link to attached file
    public required string Path { get; set; }

    public required string Filename { get; set; }

    public required string ContentType { get; set; }
}