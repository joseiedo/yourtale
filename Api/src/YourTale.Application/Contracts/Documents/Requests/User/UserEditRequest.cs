using static System.String;

namespace YourTale.Application.Contracts.Documents.Requests.User;

public class UserEditRequest
{
    public int UserId { get; set; }

    public string Nickname { get; set; } = Empty;

    public string Picture { get; set; } = Empty;
}