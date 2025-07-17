using TicketingSys.Dtos.AuthDtos;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface IAuthService
{
    public Task CreateUser(RegisterDto registerDto);

    public Task<LoginResponseDto> Login(LoginDto loginDto);
}