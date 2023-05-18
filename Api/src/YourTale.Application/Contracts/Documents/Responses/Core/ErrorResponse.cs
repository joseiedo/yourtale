namespace YourTale.Application.Contracts.Documents.Responses.Core;

public class ErrorResponse
{
    public ErrorResponse(IReadOnlyCollection<Notification> errors)
    {
        Errors = errors;
    }

    public ErrorResponse(Notification notification)
    {
        Errors = new List<Notification> { notification };
    }

    public IReadOnlyCollection<Notification> Errors { get; set; }
}