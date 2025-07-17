using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Enums;
using TicketingSys.Models;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface ISharedService
{
    Task<ViewUserDto?> GetUserById(string id);

    Task<List<Ticket>> GetAllTicketByUserId(string userId);

    Task<ViewResponseDto?> AddResponse(NewResponseDto dto, string userId);

    Task<ViewTicketDto?> GetTicketById(int ticketId, bool includeResponses);

    Task<List<ViewResponseDto>> GetResponsesSentByUser(string userId, ResponseQueryParamsDto query);

    Task<ViewResponseDto?> GetSentResponseByUserIdAndResponseId(string userId, int responseId);

    Task<List<ViewTicketDto>> GetAllTicketsFromMyDepartment(string userId);

    Task<List<ViewTicketDto>> QueryAlLTicketsFromMyDepartment(string currentUserId,
        SharedTicketQueryParamsDto query);

    Task<List<ViewTicketDto>> GetAllTicketsFromUserByDepartment(string userId);

    Task<ViewTicketDto?> ChangeTicketStatus(int ticketId, TicketStatusEnum status, string userId);

    Task<ViewTicketDto?> AssignTicketToUser(int ticketId, string currentUserId);

    Task<List<ViewDepartmentDto>> GetAllDepartments();

    Task<List<ViewDepartmentDto>> GetMyAssignedDepartments(string userId);

    Task<ViewTicketCategoryDto?> GetCategoryById(int categoryId);

    Task<ViewDepartmentDto?> GetDepartmentById(int departmentId);

    Task<List<ViewTicketCategoryDto>> GetAllCategoriesFromDepartment(int departmentId);

    Task<List<ViewTicketCategoryDto>> GetAllCategories();
}