namespace TicketingSys.Exceptions;

public class InfeasibleOperationException
    : Exception
{
    private const string Default = "Could not perform requested operation.";
    public InfeasibleOperationException
        (string message) : base(message)
    {
    }

    public InfeasibleOperationException
        () : base(Default)
    {
    }

}
