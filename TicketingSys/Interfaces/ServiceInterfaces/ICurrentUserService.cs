namespace TicketingSys.Interfaces.ServiceInterfaces;

public interface ICurrentUserService
{
    string GetUserIdOr401();

    Task HasDepartmentAccessOr403(int departmentId);
}