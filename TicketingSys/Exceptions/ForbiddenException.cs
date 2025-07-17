namespace TicketingSys.Exceptions;

public class ForbiddenException : Exception
{
    private const string Default = "You do not have access to this resource.";

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException() : base(Default)
    {
    }
}