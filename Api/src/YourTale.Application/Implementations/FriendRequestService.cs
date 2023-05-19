using AutoMapper;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class FriendRequestService : IFriendRequestService
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public FriendRequestService(IMapper mapper, IUserService userService, IUserRepository userRepository,
        IFriendRequestRepository friendRequestRepository)
    {
        _mapper = mapper;
        _userService = userService;
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
    }

    public async Task<AddFriendResponse> AddFriend(int friendId)
    {
        var response = new AddFriendResponse();

        var user = _userService.GetAuthenticatedUser();
        var friend = _userRepository.GetUserById(friendId);

        if (friend is null)
        {
            response.AddNotification(new Notification("Amigo não encontrado"));
            return response;
        }

        if (_friendRequestRepository.FriendRequestAlreadyExists(user.Id, friendId))
        {
            response.AddNotification(new Notification("Solicitação de amizade já enviada"));
            return response;
        }

        var friendRequest = new FriendRequest
        {
            User = user,
            Friend = friend
        };

        var entity = await _friendRequestRepository.Add(friendRequest);

        response.FriendRequest = _mapper.Map<FriendRequestDto>(entity);

        return response;
    }

    public async Task<AcceptFriendResponse> AcceptFriendRequest(int friendRequestId)
    {
        var response = new AcceptFriendResponse();
        var friendRequest = _friendRequestRepository.GetById(friendRequestId);

        if (friendRequest is null)
        {
            response.AddNotification(new Notification("Solicitação de amizade inválida"));
            return response;
        }

        if (friendRequest.IsAccepted)
        {
            response.AddNotification(new Notification("Solicitação de amizade já aceita"));
            return response;
        }

        friendRequest.AcceptedAt = DateTime.Now;
        friendRequest.IsAccepted = true;

        await _friendRequestRepository.SaveAllChanges();

        response.FriendRequest = _mapper.Map<FriendRequestDto>(friendRequest);

        return response;
    }

    public DeclineFriendResponse DeclineFriendRequest(int friendRequestId)
    {
        var response = new DeclineFriendResponse();
        var friendRequest = _friendRequestRepository.GetById(friendRequestId);

        if (friendRequest is null)
        {
            response.AddNotification(new Notification("Solicitação de amizade inválida"));
            return response;
        }
            

        friendRequest.RejectedAt = DateTime.Now;
        _friendRequestRepository.SaveAllChanges();
        
        response.FriendRequest = _mapper.Map<FriendRequestDto>(friendRequest);

        return response;
    }

    public async Task<List<FriendRequestDto>> GetFriendRequests()
    {
        var user = _userService.GetAuthenticatedUser();
        var friendRequests = await _friendRequestRepository.GetFriendRequests(user.Id);

        return _mapper.Map<List<FriendRequestDto>>(friendRequests);
    }

    public async Task<Pageable<UserDto>> GetFriendsByNameOrEmailEquals(string text, int page, int take)
    {
        var user = _userService.GetAuthenticatedUser();
        var friends = await _friendRequestRepository.GetFriendsByFullNameOrEmailEqual(user.Id, text, page, take);

        return new Pageable<UserDto>
        {
            Content = _mapper.Map<List<UserDto>>(friends),
            Page = page,
            IsLastPage = friends.Count < take
        };

    }
}