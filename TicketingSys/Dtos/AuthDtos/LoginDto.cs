using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.AuthDtos;

public record LoginDto([Required] string Email, [Required] string Password);