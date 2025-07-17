namespace TicketingSys.Dtos.AuthDtos;

public record LoginResponseDto
{
    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Token { get; init; }
}