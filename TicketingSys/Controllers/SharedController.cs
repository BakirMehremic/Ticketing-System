using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Utils;

namespace TicketingSys.Controllers;

// NOTE:
// routes that can be used by multiple user roles are here
[Route("shared")]
[ApiController]
public class SharedController(
    ISharedService sharedService,
    ICurrentUserService currentUserService) : ControllerBase
{
    
    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpGet("tickets")]
    public async Task<ActionResult<List<ViewTicketDto>>> GetAllTicketsWithQueryOptional(
        [FromQuery] SharedTicketQueryParamsDto? query)
    {
        string currentUserId = currentUserService.GetUserIdOr401();

        List<ViewTicketDto> tickets = query == null || query.IsEmpty()
            ? await sharedService.GetAllTicketsFromMyDepartment(currentUserId)
            : await sharedService.QueryAlLTicketsFromMyDepartment(currentUserId, query);

        return Ok(tickets);
    }

    [Authorize(Policy = "AdminOrDepartmentUser")]
    [HttpGet("tickets/{ticketId}")]
    public async Task<ActionResult<ViewTicketDto?>> GetTicketById(int ticketId)
    {

        try
        {
             ViewTicketDto? ticket = await sharedService.GetTicketById(ticketId, true);
             return ticket == null ? NotFound() : Ok(ticket);
        } 
        catch (ForbiddenException ex) // if user tries to access ticket outside their department
        {
            return Forbid(ex.Message);
        }

    }

    [Authorize("AdminOrDepartmentUser")]
    [HttpGet("users/{userId}/tickets")]
    public async Task<ActionResult<List<ViewTicketDto>>> GetAllUsersTickets(string userId)
    {
        List<ViewTicketDto> tickets = await sharedService.GetAllTicketsFromUserByDepartment(userId);

        return Ok(tickets);
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpPatch("tickets/{ticketId}/status")]
    public async Task<ActionResult<ViewTicketDto?>> ChangeTicketStatus(int ticketId,
        [FromBody] ChangeTicketStatusDto status)
    {
        string currentUserId = currentUserService.GetUserIdOr401();

        ViewTicketDto? ticket = await sharedService.ChangeTicketStatus(ticketId, status.Status, currentUserId);

        return ticket == null ? NotFound() : Ok(ticket);
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpPatch("tickets/{ticketId}/assign")]
    public async Task<ActionResult<ViewTicketDto?>> AssignTicketToMe(int ticketId)
    {
        string currentUserId = currentUserService.GetUserIdOr401();

        ViewTicketDto? ticket = await sharedService.AssignTicketToUser(ticketId, currentUserId);

        return ticket == null ? NotFound() : Ok(ticket);
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpPost("responses")]
    public async Task<IActionResult> AddResponse([FromBody] NewResponseDto dto)
    {
        string userId = currentUserService.GetUserIdOr401();

        ViewResponseDto? savedResponse = await sharedService.AddResponse(dto, userId);

        if (savedResponse == null)
        {
            return BadRequest("Invalid ticket id");
        }

        return CreatedAtAction(
            nameof(GetSentResponseById),
            new { responseId = savedResponse.Id },
            savedResponse
        );
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpGet("responses/sent")]
    public async Task<ActionResult<List<ViewResponseDto>>> GetSentResponses([FromQuery]
        ResponseQueryParamsDto query)
    {
        string userId = currentUserService.GetUserIdOr401();

        List<ViewResponseDto> results = await sharedService.GetResponsesSentByUser(userId, query);

        return Ok(results);
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpGet("responses/{responseId}")]
    public async Task<ActionResult<ViewResponseDto?>> GetSentResponseById(int responseId)
    {
        string userId = currentUserService.GetUserIdOr401();

        ViewResponseDto? response = await sharedService.
            GetSentResponseByUserIdAndResponseId(userId, responseId);

        return response == null ? 
            NotFound("You did not send a response with this id") : Ok(response);

    }

    [Authorize]
    [HttpGet("categories")]
    public async Task<ActionResult<List<ViewTicketCategoryDto>>> GetAllCategories()
    {
        List<ViewTicketCategoryDto> result = await sharedService.GetAllCategories();

        return Ok(result);
    }

    [Authorize]
    [HttpGet("categories/{categoryId}")]
    public async Task<ActionResult<ViewDepartmentDto?>> GetCategoryById(int categoryId)
    {
        ViewTicketCategoryDto? result = await sharedService.GetCategoryById(categoryId);

        return result == null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpGet("departments/{deptId}/categories")]
    public async Task<ActionResult<List<ViewTicketCategoryDto>>> GetAllCategoriesById(int deptId)
    {
        List<ViewTicketCategoryDto> result = await sharedService.GetAllCategoriesFromDepartment(deptId);
        
        return Ok(result);
    }

    [Authorize]
    [HttpGet("departments")]
    public async Task<ActionResult<List<ViewDepartmentDto>>> GetAllDepartments()
    {
        List<ViewDepartmentDto> results = await sharedService.GetAllDepartments();

        return Ok(results);
    }

    [Authorize(Policy = "DepartmentUserOnly")]
    [HttpGet("departments/my")]
    public async Task<ActionResult<List<ViewDepartmentDto>>> GetMyAssignedDepartments()
    {
        string userId = currentUserService.GetUserIdOr401();

        List<ViewDepartmentDto> results = await sharedService.GetMyAssignedDepartments(userId);

        return Ok(results);
    }

    [Authorize]
    [HttpGet("departments/{departmentId}")]
    public async Task<ActionResult<ViewDepartmentDto?>> GetDepartmentById(int departmentId)
    {
        ViewDepartmentDto? result = await sharedService.GetDepartmentById(departmentId);

        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Policy = "AdminOrDepartmentUser")]
    [HttpGet("users/{userId}")]
    public async Task<ActionResult<ViewUserDto>> GetProfileById(string userId)
    {
        ViewUserDto? user = await sharedService.GetUserById(userId);

        return user == null
            ? NotFound(new { message = "User profile not found." })
            : Ok(user);
    }

    [Authorize]
    [HttpGet("users/me")]
    public async Task<ActionResult<ViewUserDto>> MyProfile()
    {
        string userId = currentUserService.GetUserIdOr401();

        ViewUserDto? user = await sharedService.GetUserById(userId);

        return user == null
            ? NotFound(new { message = "User profile not found." })
            : Ok(user);
    }
}