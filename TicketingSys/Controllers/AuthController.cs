using Microsoft.AspNetCore.Mvc;
using TicketingSys.Dtos.AuthDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.ServiceInterfaces;

namespace TicketingSys.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            await authService.CreateUser(registerDto);

            return Ok("Successfully registered.");
        }
        catch (UniqueConstraintFailedException ex)
        {
            return Conflict(ex.Message);
        }
        catch (InfeasibleOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            LoginResponseDto response = await authService.Login(dto);
            return Ok(response);
        }
        catch (InvalidLoginCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}