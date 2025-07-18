﻿namespace TicketingSys.Models;

public class UserDepartmentAccess 
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; }
}