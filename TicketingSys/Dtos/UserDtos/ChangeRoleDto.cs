namespace TicketingSys.Dtos.UserDtos;

public record ChangeRoleDto(bool? IsAdmin, List<int>? DepartmentIds);