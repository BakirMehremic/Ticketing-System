using System.ComponentModel.DataAnnotations;

namespace TicketingSys.Dtos.CategoryDtos;

public record NewTicketCategoryDto(
    [Required] string Name,
    string? Description,
    [Required] int DepartmentId);