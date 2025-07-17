using TicketingSys.Enums;

namespace TicketingSys.Models;

public class Ticket 
{
    public int Id { get; set; }

    // references the user who submitted the ticket
    public string SubmittedById { get; set; }

    public User SubmittedBy { get; set; }

    // references the user a ticket is assigned to
    public string? AssignedToId { get; set; }

    public User? AssignedTo { get; set; }

    // no hard coded categories all will be saved in category table
    public int CategoryId { get; set; }

    public TicketCategory Category { get; set; }

    // no hard coded departments
    public int DepartmentId { get; set; }

    public Department Department { get; set; }

    // list of attached files to ticket - optional
    public List<TicketAttachment> Attachments { get; set; } = new();

    public TicketStatusEnum Status { get; set; }

    public string Title { get; set; }

    public TicketUrgencyEnum Urgency { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; }

    public string Description { get; set; }

    public List<Response>? Responses { get; set; } = new();
}