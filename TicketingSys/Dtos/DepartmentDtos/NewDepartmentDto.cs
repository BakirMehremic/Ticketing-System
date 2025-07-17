using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.DepartmentDtos;

public record NewDepartmentDto([Required] string NewDepartmentName);
