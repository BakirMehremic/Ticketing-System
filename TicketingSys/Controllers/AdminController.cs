using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSys.Dtos.CategoryDtos;
using TicketingSys.Dtos.DepartmentDtos;
using TicketingSys.Dtos.UserDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Utils;

namespace TicketingSys.Controllers;

[Route("admin")]
[ApiController]
[Authorize(Policy = "AdminOnly")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<ActionResult<List<ViewUserDto>>> ViewAllUsersWithQueryOptional([FromQuery] 
        UserQueryParamsDto? query)
    {
        List<ViewUserDto> users = query == null || query.IsEmpty()
            ? await adminService.GetAllUsers()
            : await adminService.QueryUsers(query);

        return Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<ViewUserDto>> ViewUser(string userId)
    {
        ViewUserDto? user = await adminService.GetUserById(userId);

        return user == null ? NotFound() : Ok(user);
    }

    [HttpPatch("users/{userId}/role")]
    public async Task<ActionResult<ViewUserDto>> ChangeUserRole([FromBody] ChangeRoleDto dto,
        string userId)
    {
        if (dto.IsAdmin == null && dto.DepartmentIds == null)
        {
            return BadRequest("No valid changes provided");
        }

        ViewUserDto? user = await adminService.ChangeRole(dto, userId);

        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost("departments")]
    public async Task<ActionResult> AddDepartment([FromBody] NewDepartmentDto dto)
    {
        try
        {
            ViewDepartmentDto newDept = await adminService.AddDepartment(dto.NewDepartmentName);

            return CreatedAtAction(
                actionName: nameof(SharedController.GetDepartmentById),
                controllerName: "Shared",
                routeValues: new { departmentId = newDept.Id },
                value: newDept
            );
        }
        catch (UniqueConstraintFailedException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("categories")]
    public async Task<ActionResult<NewTicketCategoryDto>> AddCategory([FromBody] NewTicketCategoryDto dto)
    {
        try
        {
            ViewTicketCategoryDto newCategory = await adminService.AddCategory(dto);

            return CreatedAtAction(actionName:
                nameof(SharedController.GetCategoryById),
                controllerName: "Shared",
                routeValues: new { categoryId = newCategory.Id },
                value: newCategory);
        }
        catch (UniqueConstraintFailedException ex)
        {
            return Conflict(ex.Message);
        }
        catch (InfeasibleOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            await adminService.DeleteCategoryById(id);
            return NoContent();
        }
        catch (InfeasibleOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("departments/{id}")]
    public async Task<ActionResult> DeleteDepartment(int id)
    {
        try
        {
            await adminService.DeleteDepartmentById(id);
            return NoContent();
        }
        catch (InfeasibleOperationException ex)
        {
            return Conflict(ex.Message); 
        }
    }
}