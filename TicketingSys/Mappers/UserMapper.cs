using System.Text;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Models;

namespace TicketingSys.Mappers;

public static class UserMapper
{
    public static ViewUserDto UserModelToDto(this User userModel)
    {
        return new ViewUserDto
        {
            UserId = userModel.Id,
            FullName = new StringBuilder(userModel.FirstName)
                .Append(' ')
                .Append(userModel.LastName)
                .ToString(),
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            IsAdmin = userModel.IsAdmin
        };
    }

    public static ViewUserDto UserModelToDto(this User userModel, List<UserDepartmentAccess> userDepartmentAccessList)
    {
        List<AccessibleDepartmentDto> deptAccessListDtos = userDepartmentAccessList.Select(da =>
            new AccessibleDepartmentDto
            {
                Id = da.Id,
                Name = da.Department.Name
            }).ToList();

        return new ViewUserDto
        {
            UserId = userModel.Id,
            FullName = new StringBuilder(userModel.FirstName)
                .Append(' ')
                .Append(userModel.LastName)
                .ToString(),
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            IsAdmin = userModel.IsAdmin
        };
    }
}