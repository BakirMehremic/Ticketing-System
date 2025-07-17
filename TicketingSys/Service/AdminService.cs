using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Mappers;
using TicketingSys.Models;
using TicketingSys.QueryBuilders;
using TicketingSys.Redis;
using TicketingSys.Settings;

namespace TicketingSys.Service;

public class AdminService(
    IUserAccessCacheService redisService,
    IUserRepository userRepository,
    IDepartmentAccessRepository departmentAccessRepository,
    IDepartmentRepository departmentRepository,
    ITicketCategoryRepository ticketCategoryRepository)
    : IAdminService
{
    public async Task<List<ViewUserDto>> GetAllUsers()
    {
        List<User> users = await userRepository.GetAll();

        List<UserDepartmentAccess> allAccess = await userRepository.
            GetAllUserDepartmentAccess(includeDepartments:true);

        List<ViewUserDto> result = users.Select(user =>
        {
            List<UserDepartmentAccess> deptAccessList = allAccess
                .Where(x => x.UserId == user.Id)
                .ToList();

            return user.UserModelToDto(deptAccessList);
        }).ToList();

        return result;
    }

    public async Task<ViewUserDto?> GetUserById(string id)
    {
        User? user = await userRepository.GetUserById(id);
        if (user == null) return null;

        List<UserDepartmentAccess> deptAccessList = await userRepository.
            GetUserDepartmentAccessListById(id);

        return user.UserModelToDto(deptAccessList);
    }

    public async Task<ViewUserDto?> ChangeRole(ChangeRoleDto dto, string id)
    {
        User? user = await userRepository.GetUserById(id, includeDepartmentAccessList:true);

        if (user == null) return null;

        if (dto.IsAdmin != null)
        {
            user.IsAdmin = dto.IsAdmin.Value;
        }

        if (dto.DepartmentIds != null)
        {
            await departmentAccessRepository.DeleteDepartmentAccessesByUserId(id);

            List<UserDepartmentAccess> newAccesses = dto.DepartmentIds.Select(deptId => new UserDepartmentAccess
            {
                UserId = id,
                DepartmentId = deptId
            }).ToList();

            await departmentAccessRepository.AddList(newAccesses);
        }

        await redisService.InvalidateUserAccessAsync(id);

        ViewUserDto dtoResult = user.UserModelToDto();

        return dtoResult;
    }

    public async Task<ViewDepartmentDto> AddDepartment(string name)
    {
        Department? exists = await departmentRepository.GetDepartmentByName(name);

        if (exists != null)
        {
            throw new UniqueConstraintFailedException("Can not create two departments with same name");
        }

        Department created = await departmentRepository.Add(new Department { Name = name });

        return created.ModelToViewDto();
    }

    public async Task<ViewTicketCategoryDto> AddCategory(NewTicketCategoryDto category)
    {
        TicketCategory? exists = await ticketCategoryRepository.
            GetByNameAndDeptId(category.Name, category.DepartmentId);

        if(exists!=null)
            throw new UniqueConstraintFailedException("Category with this name exists in the selected department");

        Department? department = await departmentRepository.GetById(category.DepartmentId);

        if (department == null)
            throw new InfeasibleOperationException("Invalid department id.");

        TicketCategory newCategory = new()
        {
            DepartmentId = category.DepartmentId,
            Name = category.Name,
            Description = category.Description
        };
        
        TicketCategory created = await ticketCategoryRepository.Add(newCategory);

        return created.ModelToViewDto();
    }

    public async Task<IEnumerable<ViewTicketCategoryDto>> GetAllCategories()
    {
        List<TicketCategory> categories = await ticketCategoryRepository.GetAll();

        return categories.Select(c => c.ModelToViewDto());
    }

    public async Task<List<ViewUserDto>> QueryUsers(UserQueryParamsDto queryParams)
    {
        IQueryable<User> usersQuery = userRepository.GetQueryable();

        var builder = new UserQueryBuilder()
            .WithFirstName(queryParams.FirstName)
            .WithLastName(queryParams.LastName)
            .WithFullName(queryParams.FullName)
            .WithEmail(queryParams.Email)
            .WithIsAdmin(queryParams.IsAdmin)
            .WithHasDepartmentAccess(queryParams.HasDepartments);

        usersQuery = builder.BuildQueryable(usersQuery);

        List<User> users = await usersQuery.ToListAsync();

        return users.Select(x => x.UserModelToDto()).ToList();
    }

    public async Task DeleteCategoryById(int id)
    {
        TicketCategory? category = await ticketCategoryRepository.GetById(id);

        if (category == null) 
            throw new InfeasibleOperationException("Category you want to delete does not exist.");

        bool isReferenced = await ticketCategoryRepository.IsCategoryReferencedByTickets(id);

        if (isReferenced)
        {
            throw new InfeasibleOperationException("Cannot delete category with assigned tickets.");
        }

        await ticketCategoryRepository.Delete(category);
    }

    public async Task DeleteDepartmentById(int id)
    {
        Department? dept = await departmentRepository.GetById(id);

        if (dept == null)
            throw new InfeasibleOperationException("Department you want to delete does not exist.");

        bool canBeDeleted = await departmentRepository.CheckIfDepartmentIsReferenced(id);

        if (!canBeDeleted)
        {
            throw new InfeasibleOperationException("This department has related tickets or categories.");
        }

        await departmentRepository.Delete(dept);
    }
}