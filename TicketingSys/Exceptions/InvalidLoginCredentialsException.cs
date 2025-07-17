namespace TicketingSys.Exceptions;

public class InvalidLoginCredentialsException : Exception
{
    private const string Default = "Invalid credentials";

    public InvalidLoginCredentialsException(string message) : base(message)
    {
    }

    public InvalidLoginCredentialsException() : base(Default)
    {
    }
}