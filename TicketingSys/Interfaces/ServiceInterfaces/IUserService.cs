using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Models;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface IUserService
{
    Task<ViewTicketDto> AddNewTicket(string userId, NewTicketDto dto);

    Task<ViewTicketDto?> GetTicketBySubmittedByIdAndTicketId(string userId, int ticketId);

    Task<List<ViewTicketDto>> GetAllTicketByUserId(string userId);

    Task<List<ViewTicketDto>> FilterTickets(string userId, TicketQueryParamsDto filters);

    Task<List<ViewResponseDto>> GetResponsesToUserTickets(string userId, ResponseQueryParamsDto query);

    Task<ViewResponseDto?> GetResponseToMyTicketById(string userId, int responseId);

    Task<bool> CheckIfCategoryIsValid(int categoryId, int departmentId);
}