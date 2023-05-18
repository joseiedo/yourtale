using YourTale.Application.Contracts;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class FriendRequestService : IFriendRequestService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    
    public FriendRequestService(IUserService userService, IUserRepository userRepository, IFriendRequestRepository friendRequestRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
    }
    
    public Task SendFriendRequest(int friendId)
    {
        var user = _userService.GetAuthenticatedUser();
        var friend = _userRepository.GetUserById(friendId);

        if (friend is null)
        {
            // TODO: retornar erro
        }
       
        var friendRequest = new FriendRequest
        {
            User = user,
            Friend = friend
        };

        _friendRequestRepository.Add(friendRequest);
        
        throw new NotImplementedException();
    }

    public Task AcceptFriendRequest(int friendRequestId)
    {
        throw new NotImplementedException();
    }

    public Task DeclineFriendRequest(int friendRequestId)
    {
        throw new NotImplementedException();
    }
}