using Microsoft.EntityFrameworkCore;
using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Enums;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Mappers;
using TicketingSys.Models;
using TicketingSys.QueryBuilders;
using TicketingSys.Settings;
using TicketingSys.Utils;
using TicketCategory = TicketingSys.Models.TicketCategory;

namespace TicketingSys.Service;

public class SharedService(
    IResponseRepository responseRepository,
    ICurrentUserService currentUserService,
    ITicketRepository ticketRepository,
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository,
    ITicketCategoryRepository categoryRepository,
    IAttachmentService attachmentService) : ISharedService
{
    public async Task<ViewResponseDto?> AddResponse(NewResponseDto dto, string id)
    {
        Response response = new()
        {
            TicketId = dto.TicketId,
            UserId = id,
            Message = dto.Message,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };

        await responseRepository.Add(response);

        Ticket? referencedTicket = await ticketRepository.GetById(dto.TicketId);

        if (referencedTicket == null) return null;

        await currentUserService.HasDepartmentAccessOr403(referencedTicket.DepartmentId);

        if (referencedTicket.Status != dto.Status)
        {
            referencedTicket.Status = dto.Status;
        }

        if (dto.Attachments.Any())
        {
            List<ResponseAttachment> attachments =
                await attachmentService.CreateAndSaveResponseAttachments(response.Id, dto.Attachments);
            response.Attachments = attachments;
        }

        return response.ToViewDto();
    }

    public async Task<List<Ticket>> GetAllTicketByUserId(string id)
    {
        List<Ticket> tickets = await ticketRepository.GetAllTicketByUserId(id);

        return tickets.SortByStatusAndUrgency();
    }

    public async Task<List<ViewTicketDto>> GetAllTicketsFromMyDepartment(string id)
    {
        User? user = await userRepository.GetUserById(id);

        List<UserDepartmentAccess> ids = await userRepository.GetUserDepartmentAccessListById(id);

        List<int> departmentIds = new();
        foreach (var x in ids)
        {
            departmentIds.Add(x.DepartmentId);
        }

        List<Ticket> tickets =
            await ticketRepository.GetAllTicketsByDepartmentIds(departmentIds, includeNavProperties: true);

        List<Ticket> sorted = tickets.SortByStatusAndUrgency();
        return sorted.ModelToViewDtoList();
    }

    public async Task<List<ViewTicketDto>> QueryAlLTicketsFromMyDepartment(string currentUserId,
        SharedTicketQueryParamsDto query)
    {
        List<int> departmentIds = (await userRepository.
            GetUserDepartmentAccessListById(currentUserId))
            .Select(x => x.DepartmentId).ToList();

        var builder = new TicketQueryBuilder()
            .WithStatus(query.Status)
            .WithUrgency(query.Urgency)
            .WithCategoryName(query.CategoryName)
            .WithDepartmentName(query.DepartmentName)
            .WithFromDate(query.FromDate)
            .WithToDate(query.ToDate)
            .WithIsAssigned(query.IsAssigned)
            .WithHasResponses(query.HasResponses)
            .WithAssignedToId(query.AssignedToId)
            .WithSearch(query.Search);

        List<Ticket> ticketList = await ticketRepository.
            GetAllTicketsByDepartmentIds(departmentIds,includeNavProperties: true);

        IQueryable<Ticket> queryable = ticketList.AsQueryable();

        queryable = builder.BuildQueryable(queryable);

        List<Ticket> tickets = queryable.ToList();
        List<Ticket> sorted = tickets.SortByStatusAndUrgency();

        return sorted.ModelToViewDtoList();
    }

    public async Task<List<ViewResponseDto>> GetResponsesSentByUser(string userId, ResponseQueryParamsDto query)
    {
        List<Response> responses = await responseRepository.
            GetResponsesByUserId(userId, includeNavProperties: true);

        IQueryable<Response> queryable = responses.AsQueryable();

        var builder = new ResponseQueryBuilder()
            .WithSearch(query.Search)
            .WithStatus(query.Status)
            .WithCategoryName(query.CategoryName)
            .WithDepartmentName(query.DepartmentName)
            .WithAssignedToName(query.AssignedToName)
            .WithFromDate(query.FromDate)
            .WithToDate(query.ToDate)
            .WithHasAttachments(query.HasAttachments);

        queryable = builder.BuildQueryable(queryable);

        List<Response> resultList = await queryable.ToListAsync();
        List<Response> sorted = resultList.SortByTicketStatusAndUrgency();

        return sorted.Select(r => r.ToViewDto()).ToList();
    }

    public async Task<ViewResponseDto?> GetSentResponseByUserIdAndResponseId(string id, int responseId)
    {
        Response? response = await responseRepository.GetSentResponseByUserIdAndResponseId(id, responseId);

        return response?.ToViewDto();
    }

    public async Task<ViewTicketDto?> GetTicketById(int ticketId, bool includeResponses)
    {
        Ticket? ticket = await ticketRepository.GetById(ticketId,
            includeProperties:[ x => x.SubmittedBy, x => x.AssignedTo, x => x.Category,
                x => x.Department, x => x.Attachments, x => x.Responses, x => x.Attachments]);

        return ticket?.ModelToViewDto(includeResponses);
    }

    public async Task<ViewUserDto?> GetUserById(string id)
    {
        User? user = await userRepository.GetUserById(id);

        if (user == null) return null;

        List<UserDepartmentAccess> accessibleDepartments = await userRepository.
            GetUserDepartmentAccessListById(id);

        return user.UserModelToDto(accessibleDepartments);
    }

    public async Task<List<ViewTicketDto>?> GetAllTicketsFromUserByDepartment(string id)
    {
        User? user = await userRepository.GetUserById(id);

        if (user == null)
            throw new ForbiddenException();

        if (user.IsAdmin)
        {
            List<Ticket> allTickets = await ticketRepository.
                GetAllTicketByUserId(id, includeNavProperties: true);

            List<Ticket> sortedAdmin = allTickets.SortByStatusAndUrgency();

            return sortedAdmin.ModelToViewDtoList();
        }

        List<int> departmentIds = (await userRepository.GetUserDepartmentAccessListById(id)).
            Select(x=>x.DepartmentId).ToList();


        List<Ticket> tickets = await ticketRepository.
            GetAllTicketsByDepartmentIds(departmentIds, includeNavProperties:true);

        List<Ticket> sorted = tickets.SortByStatusAndUrgency();

        return sorted.ModelToViewDtoList();
    }

    public async Task<ViewTicketDto?> ChangeTicketStatus(int ticketId, TicketStatusEnum status, string userId)
    {
        Ticket? ticket = await ticketRepository.GetById(ticketId, includeProperties:
        [
            x => x.Department, x => x.SubmittedBy, x => x.AssignedTo, x => x.Category, x => x.Attachments
        ]);

        if (ticket == null) return null;

        bool userHasAccess = await userRepository.
            UserHasDepartmentAccess(userId, ticket.DepartmentId);

        if (!userHasAccess)
            throw new ForbiddenException("You do not have access to this department.");

        await ticketRepository.ChangeTicketStatus(status, ticketId);

        return ticket.ModelToViewDto();
    }

    public async Task<ViewTicketDto?> AssignTicketToUser(int ticketId, string id)
    {
        Ticket? ticket = await ticketRepository.GetById(ticketId, includeProperties:
            [
                x => x.Department, x => x.SubmittedBy, x => x.AssignedTo,
                x => x.Category, x => x.Attachments
            ]);

        if (ticket == null) return null;

        string departmentName = ticket.Department.Name.ToLower();

        bool userHasAccess = await userRepository.UserHasDepartmentAccess(id, ticketId);

        if (!userHasAccess)
            throw new ForbiddenException("This ticket is not in your department.");

        ticket.AssignedToId = id;
        ticket.UpdatedAt = DateTime.UtcNow;

        await ticketRepository.Update(ticket);

        return ticket.ModelToViewDto();
    }

    public async Task<List<ViewDepartmentDto>> GetAllDepartments()
    {
        List<Department> departments = await departmentRepository.GetAll();

        return departments.Select(d => new ViewDepartmentDto
        {
            Id = d.Id,
            Name = d.Name
        }).ToList();
    }

    public async Task<List<ViewDepartmentDto>> GetMyAssignedDepartments(string id)
    {
        List<UserDepartmentAccess> assignedDepts = await userRepository.GetUserDepartmentAccessListById(id);

        return assignedDepts.Select(x => x.Department.ModelToViewDto()).ToList();
    }

    public async Task<ViewDepartmentDto?> GetDepartmentById(int id)
    {
        Department? department = await departmentRepository.GetById(id);

        if (department == null) return null;

        return department.ModelToViewDto();
    }

    public async Task<ViewTicketCategoryDto?> GetCategoryById(int id)
    {
        TicketCategory? category = await categoryRepository.GetById(id);

        return category?.ModelToViewDto();
    }

    public async Task<List<ViewTicketCategoryDto>> GetAllCategoriesFromDepartment(int departmentId)
    {
        List<TicketCategory> categories = await categoryRepository.
            GetAllCategoriesFromDepartment(departmentId);

        return categories.Select(c => c.ModelToViewDto()).ToList();
    }

    public async Task<List<ViewTicketCategoryDto>> GetAllCategories()
    {
        List<TicketCategory> categories = await categoryRepository.GetAll();

        return categories.Select(c => c.ModelToViewDto()).ToList();
    }
}