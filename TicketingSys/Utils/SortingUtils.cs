using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.Utils;

public static class SortingUtils
{
    public static List<Ticket> SortByStatusAndUrgency(this IEnumerable<Ticket> tickets)
    {
        return tickets
            .Where(t => t.Status != TicketStatusEnum.Deleted)
            .OrderBy(t =>
                t.Status == TicketStatusEnum.Open || t.Status == TicketStatusEnum.Reopened ? 0 :
                t.Status == TicketStatusEnum.InProgress || t.Status == TicketStatusEnum.Resolved ? 1 :
                t.Status == TicketStatusEnum.Closed ? 2 : 3)
            .ThenByDescending(t => t.Urgency)
            .ToList();
    }

    public static List<Response> SortByTicketStatusAndUrgency(this IEnumerable<Response> responses)
    {
        return responses
            .Where(r => r.Ticket != null && r.Ticket.Status != TicketStatusEnum.Deleted)
            .OrderBy(r =>
                r.Ticket.Status == TicketStatusEnum.Open || r.Ticket.Status == TicketStatusEnum.Reopened ? 0 :
                r.Ticket.Status == TicketStatusEnum.InProgress || r.Ticket.Status == TicketStatusEnum.Resolved ? 1 :
                r.Ticket.Status == TicketStatusEnum.Closed ? 2 : 3)
            .ThenByDescending(r => r.Ticket.Urgency)
            .ToList();
    }
}