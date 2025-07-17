using Azure;
using Microsoft.EntityFrameworkCore;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;
using Response = TicketingSys.Models.Response;

namespace TicketingSys.Repository;

public sealed class ResponseRepository(ApplicationDbContext context) :
    BaseGenericRepository<Response>(context), IResponseRepository
{
    public async Task<List<Response>> GetResponsesByUserId(string userId, bool includeNavProperties = true)
    {
        return includeNavProperties
            ? await context.Responses
                .Include(r => r.Ticket)
                .ThenInclude(t => t.Category)
                .Include(r => r.Ticket)
                .ThenInclude(t => t.Department)
                .Include(r => r.Ticket)
                .ThenInclude(t => t.AssignedTo)
                .Include(r => r.Ticket)
                .ThenInclude(t => t.SubmittedBy)
                .Include(r => r.Ticket)
                .ThenInclude(t => t.Attachments)
                .Include(r => r.User)
                .Include(r => r.Attachments)
                .Where(r => r.UserId == userId) // only those made by the user
                .ToListAsync()
            : await context.Responses.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Response?> GetSentResponseByUserIdAndResponseId(string id, int responseId)
    {
        return await context.Responses
            .Include(r => r.Ticket)
            .Include(r => r.User)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.UserId == id && r.Id == responseId);
    }

    public async Task<Response?> GetResponseBySubmittedByAndResponseId(string userId, int responseId,
        bool includeNavProperties = true)
    {
        return await context.Responses
            .Include(r => r.Ticket)
            .Include(r => r.User)
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Ticket.SubmittedById == userId && r.Id == responseId);
    }
}