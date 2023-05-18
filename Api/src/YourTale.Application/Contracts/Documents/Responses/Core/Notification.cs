namespace YourTale.Application.Contracts.Documents.Responses.Core;

public class Notification
{
    public Notification(string message)
    {
        Message = message;
    }

    public string Message { get; }
}