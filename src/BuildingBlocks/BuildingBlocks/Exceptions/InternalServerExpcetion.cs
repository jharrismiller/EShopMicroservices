namespace BuildingBlocks.Exceptions;

public class InternalServerExpcetion : Exception
{
    public InternalServerExpcetion(string message) : base(message)
    {

    }

    public InternalServerExpcetion(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details { get; private set; }
}
