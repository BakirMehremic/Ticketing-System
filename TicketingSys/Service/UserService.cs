using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Mappers;
using TicketingSys.Models;
using TicketingSys.QueryBuilders;
using TicketingSys.Settings;
using TicketingSys.Utils;

namespace TicketingSys.Service;

public class UserService(
    ITicketRepository ticketRepository,
    ITicketCategoryRepository categoryRepository,
    IAttachmentService attachmentService,
    IResponseRepository responseRepository) : IUserService
{
    public async Task<ViewTicketDto> AddNewTicket(string userId, NewTicketDto dto)
    {
        bool isValidCategory = await this.CheckIfCategoryIsValid(dto.CategoryId, dto.DepartmentId);

        if (!isValidCategory)
            throw new InfeasibleOperationException("Department and category for this ticket are invalid.");

        Ticket created = dto.NewDtoToModel(userId);

        await ticketRepository.Add(created);

        if (dto.Attachments.Any())
        {
            List<TicketAttachment> attachments =
                await attachmentService.CreateAndSaveTicketAttachments(created.Id, dto.Attachments);
            created.Attachments = attachments;
        }

        return created.ModelToViewDto();
    }

    public async Task<ViewTicketDto?> GetTicketBySubmittedByIdAndTicketId(string userId, int ticketId)
    {
        Ticket? ticket = await ticketRepository.GetTicketBySubmittedByIdAndTicketId(userId, ticketId);

        return ticket?.ModelToViewDto(includesResponses:true);
    }

    public async Task<List<ViewTicketDto>> GetAllTicketByUserId(string userId)
    {
        List<Ticket> tickets = await ticketRepository.GetAllTicketByUserId(userId);

        return tickets.SortByStatusAndUrgency()
            .Select(x => x.ModelToViewDto()).ToList();
    }

    public async Task<List<ViewTicketDto>> FilterTickets(string userId, TicketQueryParamsDto filters)
    {
        List<Ticket> tickets = await ticketRepository.GetAllWithProperties();

        var builder = new TicketQueryBuilder().WithIsAssigned(filters.IsAssigned).WithCategoryName(filters.CategoryName)
            .WithHasResponses(filters.HasResponses).WithStatus(filters.Status).WithUrgency(filters.Urgency)
            .WithDepartmentName(filters.DepartmentName).WithFromDate(filters.FromDate).WithToDate(filters.ToDate)
            .WithSearch(filters.Search);

        IQueryable < Ticket > queried = builder.BuildQueryable(tickets.AsQueryable());

        List<Ticket> sorted = queried.SortByStatusAndUrgency();

        return sorted.Select(t => t.ModelToViewDto(true)).ToList();
    }

    public async Task<List<ViewResponseDto>> GetResponsesToUserTickets(string userId,
        ResponseQueryParamsDto query)
    {
        List<Response> responses = await responseRepository.
            GetResponsesByUserId(userId, includeNavProperties: true);

        var builder = new ResponseQueryBuilder().WithAssignedToName(query.AssignedToName)
            .WithCategoryName(query.CategoryName).WithDepartmentName(query.DepartmentName)
            .WithFromDate(query.FromDate).WithHasAttachments(query.HasAttachments)
            .WithSearch(query.Search).WithStatus(query.Status).WithToDate(query.ToDate);

        IQueryable<Response> queried = builder.BuildQueryable(responses.AsQueryable());

        List<Response> sorted = queried.SortByTicketStatusAndUrgency();

        return sorted.Select(r => r.ToViewDto()).ToList();
    }

    public async Task<ViewResponseDto?> GetResponseToMyTicketById(string userId, int responseId)
    {
        Response? response =
            await responseRepository.GetResponseBySubmittedByAndResponseId(userId, responseId,
                includeNavProperties: true);

        return response?.ToViewDto();
    }

    public async Task<bool> CheckIfCategoryIsValid(int categoryId, int departmentId)
    {
        TicketCategory? category = await categoryRepository.GetById(categoryId);

        return category!= null && category.DepartmentId==departmentId;
    }
}