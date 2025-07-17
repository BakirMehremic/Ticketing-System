using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSys.Dtos.ResponseDtos;
using TicketingSys.Dtos.TicketDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Mappers;
using TicketingSys.Models;
using TicketingSys.Utils;

namespace TicketingSys.Controllers;

[Authorize(Policy = "RegularUserOnly")]
[Route("user")]
[ApiController]
public class UserController(
    IUserService userService,
    ICurrentUserService currentUserService) : ControllerBase
{
    [HttpPost("tickets")]
    public async Task<ActionResult<ViewTicketDto>> NewTicket([FromBody] NewTicketDto dto)
    {
        try
        {
            string userId = currentUserService.GetUserIdOr401();

            ViewTicketDto savedTicket = await userService.AddNewTicket(userId, dto);

            return CreatedAtAction(
                nameof(GetMyTicketById),
                new { ticketId = savedTicket.Id },
                savedTicket
            );
        }
        catch (InfeasibleOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("tickets/{ticketId}")]
    public async Task<ActionResult<ViewTicketDto>> GetMyTicketById(int ticketId)
    {
        string userId = currentUserService.GetUserIdOr401();

        ViewTicketDto? ticket = await userService.
            GetTicketBySubmittedByIdAndTicketId(userId, ticketId);

        return ticket == null ? NotFound() : Ok(ticket);
    }

    [HttpGet("tickets")]
    public async Task<ActionResult<List<ViewTicketDto>>> GetMyTickets([FromQuery]
        TicketQueryParamsDto? query)
    {
        string userId = currentUserService.GetUserIdOr401();

        List<ViewTicketDto> results;

        results = query == null || query.IsEmpty()
            ? await userService.GetAllTicketByUserId(userId)
            : await userService.FilterTickets(userId, query);

        return Ok(results);
    }

    [HttpGet("responses")]
    public async Task<ActionResult<List<ViewResponseDto>>> GetMyResponses([FromQuery]
        ResponseQueryParamsDto query)
    {
        string userId = currentUserService.GetUserIdOr401();

        List<ViewResponseDto> results = await userService.GetResponsesToUserTickets(userId, query);

        return Ok(results);
    }

    [HttpGet("responses/{responseId}")]
    public async Task<ActionResult<ViewResponseDto?>> GetReceivedResponse(int responseId)
    {
        string userId = currentUserService.GetUserIdOr401();

        ViewResponseDto? response = await userService.GetResponseToMyTicketById(userId, responseId);

        return response == null ? NotFound() : Ok(response);
    }
}