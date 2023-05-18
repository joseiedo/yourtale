namespace YourTale.Application.Contracts;

public interface IFriendRequestService
{
   
    Task SendFriendRequest(int friendId);
    
    Task AcceptFriendRequest(int friendRequestId);
    
    Task DeclineFriendRequest(int friendRequestId);
    
    
}