using TicketingSys.Models;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface ITokenService
{
    string CreateToken(User user);
}