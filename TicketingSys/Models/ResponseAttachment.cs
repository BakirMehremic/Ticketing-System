namespace TicketingSys.Models;

[Tags("ResponseAttachments")]
public class ResponseAttachment 
{
    public int Id { get; set; }

    public int ResponseId { get; set; }

    public string Path { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }
}