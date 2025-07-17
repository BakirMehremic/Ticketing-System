using TicketingSys.Enums;

namespace TicketingSys.Models;

public class Response 
{
    public int Id { get; set; }

    public Ticket Ticket { get; set; }

    public int TicketId { get; set; }

    // references the user who sent the response not the one who submitted the ticket
    public User User { get; set; }

    public string UserId { get; set; }

    public List<ResponseAttachment> Attachments { get; set; } = new();

    public string Message { get; set; }

    public TicketStatusEnum Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}