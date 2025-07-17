using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.UserDtos;

namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface IAdminService
{
    Task<List<ViewUserDto>> GetAllUsers();

    Task<List<ViewUserDto>> QueryUsers(UserQueryParamsDto queryParams);

    Task<ViewUserDto?> GetUserById(string userId);

    Task<ViewUserDto?> ChangeRole(ChangeRoleDto dto, string userId);

    Task<ViewDepartmentDto> AddDepartment(string name);

    Task<ViewTicketCategoryDto> AddCategory(NewTicketCategoryDto category);

    Task<IEnumerable<ViewTicketCategoryDto>> GetAllCategories();

    Task DeleteCategoryById(int id);

    Task DeleteDepartmentById(int id);
}