using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Models;

namespace TicketingSys.Mappers;

public static class DepartmentMapper
{
    public static ViewDepartmentDto ModelToViewDto(this Department department)
    {
        return new ViewDepartmentDto { Id = department.Id, Name = department.Name };
    }
}