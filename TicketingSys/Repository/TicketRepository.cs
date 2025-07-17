using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketingSys.Enums;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Repository;

public class TicketRepository(ApplicationDbContext context) :
    BaseGenericRepository<Ticket>(context), ITicketRepository
{
    public async Task<List<Ticket>> GetAllTicketByUserId(string id, bool includeNavProperties=true)
    {
        if(includeNavProperties)
            return await context.Tickets
                .Where(t => t.SubmittedById == id)
                .Include(t => t.Category)
                .Include(t => t.Department)
                .Include(t => t.AssignedTo)
                .Include(t => t.SubmittedBy)
                .Include(t => t.Attachments)
                .ToListAsync();


        return await context.Tickets.
            Where(x => x.SubmittedById == id).ToListAsync();
    }

    public async Task<List<Ticket>> GetAllTicketsByDepartmentIds(List<int> ids, bool includeNavProperties = true)
    {
        if(includeNavProperties)
            return await context.Tickets
                .Include(t => t.SubmittedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Category)
                .Include(t => t.Department)
                .Include(t => t.Attachments)
                .Where(t => ids.Contains(t.DepartmentId))
                .ToListAsync();

        return await context.Tickets.Where(t => ids.Contains(t.DepartmentId))
            .ToListAsync();
    }

    public async Task ChangeTicketStatus(TicketStatusEnum status, int ticketId)
    {
        Ticket? toUpdate = await this.GetById(ticketId);

        // should be already checked in service layer so it is not null
        if (toUpdate == null) return;

        toUpdate.Status=status;
        toUpdate.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public async Task<Ticket?> GetTicketBySubmittedByIdAndTicketId(string userId, int ticketId)
    { 
        return await context.Tickets.Include(t => t.SubmittedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Category)
            .Include(t => t.Department)
            .Include(t => t.Attachments)
            .Include(t => t.Responses)
            .ThenInclude(r => r.Attachments) // include response attachments too
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.SubmittedById == userId);
    }

    public async Task<List<Ticket>> GetAllWithProperties()
    {
        return await context.Tickets
            .Include(t => t.SubmittedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Category)
            .Include(t => t.Department)
            .Include(t => t.Attachments)
            .Include(t => t.Responses)
            .ToListAsync();
    }
}
