using System.ComponentModel.DataAnnotations;
using TicketingSys.Enums;

namespace TicketingSys.Dtos.TicketDtos;

public record ChangeTicketStatusDto([Required] TicketStatusEnum Status);