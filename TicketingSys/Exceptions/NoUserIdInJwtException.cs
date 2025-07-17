namespace TicketingSys.Exceptions;

public class NoUserIdInJwtException : Exception
{
    private const string Default = "Could not get User ID";

    public NoUserIdInJwtException(string message) : base(message)
    {
    }

    public NoUserIdInJwtException() : base(Default)
    {
    }
}