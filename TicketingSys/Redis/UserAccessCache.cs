namespace TicketingSys.Redis;

// model of what will be stored in redis for each user
public record UserAccessCacheModel(bool IsAdmin, bool HasDepartmentAccess);