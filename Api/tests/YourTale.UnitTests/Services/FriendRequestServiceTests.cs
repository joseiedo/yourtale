using AutoMapper;
using FluentAssertions;
using Moq;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Implementations;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.UnitTests.Fakers;

namespace YourTale.UnitTests.Services;

public class FriendRequestServiceTests
{
    private readonly FriendRequestFaker _friendRequestFaker = new();
    private readonly Mock<IFriendRequestRepository> _friendRequestRepository = new();
    private readonly IFriendRequestService _friendRequestService;
    private readonly Mock<IMapper> _mapper = new();
    private readonly UserFakers _userFakers = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IUserService> _userService = new();


    public FriendRequestServiceTests()
    {
        _friendRequestService = new FriendRequestService(
            _mapper.Object,
            _userService.Object,
            _userRepository.Object,
            _friendRequestRepository.Object
        );
    }

    [Fact]
    public async Task AddFriend_WhenFriendNotFound_ShouldReturnNotification()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        const int friendId = 1;

        _userService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        _userRepository.Setup(x => x.GetUserById(friendId)).Returns((User?)null);

        // Act
        var response = await _friendRequestService.AddFriend(friendId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _userService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        _userRepository.Verify(x => x.GetUserById(friendId), Times.Once);
        _friendRequestRepository.Verify(x => x.Add(It.IsAny<FriendRequest>()), Times.Never);
    }

    [Fact]
    public async Task AddFriend_WhenFriendIsCurrentUser_ShouldReturnNotification()
    {
        // Arrange
        const int friendId = 1;
        var user = _userFakers.User.RuleFor(x => x.Id, friendId).Generate();
        var friend = _userFakers.User.RuleFor(x => x.Id, friendId).Generate();

        _userService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        _userRepository.Setup(x => x.GetUserById(friendId)).Returns(friend);

        // Act
        var response = await _friendRequestService.AddFriend(friendId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _userService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        _userRepository.Verify(x => x.GetUserById(friendId), Times.Once);
        _friendRequestRepository.Verify(x => x.Add(It.IsAny<FriendRequest>()), Times.Never);
    }

    [Fact]
    public async Task AddFriend_WhenFriendRequestAlreadyRequested_ShouldReturnNotification()
    {
        // Arrange
        var user = _userFakers.User.RuleFor(x => x.Id, 1).Generate();
        var friend = _userFakers.User.RuleFor(x => x.Id, 2).Generate();

        _userService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        _userRepository.Setup(x => x.GetUserById(friend.Id)).Returns(friend);
        _friendRequestRepository.Setup(x => x.FriendRequestAlreadyExists(user.Id, friend.Id)).Returns(true);

        // Act
        var response = await _friendRequestService.AddFriend(friend.Id);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _userService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        _userRepository.Verify(x => x.GetUserById(friend.Id), Times.Once);
        _friendRequestRepository.Verify(x => x.Add(It.IsAny<FriendRequest>()), Times.Never);
    }

    [Fact]
    public async Task AddFriend_Valid_ShouldAddFriendRequest()
    {
        // Arrange
        var user = _userFakers.User.RuleFor(x => x.Id, 1).Generate();
        var friend = _userFakers.User.RuleFor(x => x.Id, 2).Generate();
        var dto = _friendRequestFaker.FriendRequestDto.Generate();

        _userService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        _userRepository.Setup(x => x.GetUserById(friend.Id)).Returns(friend);
        _friendRequestRepository.Setup(x => x.FriendRequestAlreadyExists(user.Id, friend.Id)).Returns(false);
        _mapper.Setup(s => s.Map<FriendRequestDto>(It.IsAny<FriendRequest>())).Returns(dto);


        // Act
        var response = await _friendRequestService.AddFriend(friend.Id);

        // Assert
        response.Notifications.Should().BeEmpty();
        response.FriendRequest.Should().NotBeNull();

        _userService.Verify(x => x.GetAuthenticatedUser(), Times.Once);
        _userRepository.Verify(x => x.GetUserById(friend.Id), Times.Once);
        _friendRequestRepository.Verify(x => x.Add(It.IsAny<FriendRequest>()), Times.Once);
    }

    [Fact]
    public async Task AcceptFriendRequest_whenFriendRequestInvalid_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;

        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns((FriendRequest?)null);


        // Act
        var response = await _friendRequestService.AcceptFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _friendRequestRepository.Verify(v => v.SaveAllChanges(), Times.Never);
        _friendRequestRepository.Verify(v => v.GetById(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task AcceptFriendRequest_WhenfriendRequestIsNotForCurrentUser_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;
        var friendRequest = new FriendRequest
        {
            FriendId = 1
        };
        var user = _userFakers.User.RuleFor(x => x.Id, 2).Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);


        // Act
        var response = await _friendRequestService.AcceptFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _friendRequestRepository.Verify(v => v.SaveAllChanges(), Times.Never);
        _userService.Verify(v => v.GetAuthenticatedUser(), Times.Once);
    }

    [Fact]
    public async Task AcceptFriendRequest_WhenFriendRequestAlreadyAccepted_shouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;
        var friendRequest = new FriendRequest
        {
            FriendId = 1,
            IsAccepted = true
        };
        var user = _userFakers.User.RuleFor(x => x.Id, 1).Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);


        // Act
        var response = await _friendRequestService.AcceptFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();

        _friendRequestRepository.Verify(v => v.SaveAllChanges(), Times.Never);
    }

    [Fact]
    public async Task AcceptFriendRequest_WhenValid_ShouldReturnResponse()
    {
        // Arrange
        const int searchedId = 1;
        var dto = _friendRequestFaker.FriendRequestDto.Generate();
        var friendRequest = new FriendRequest
        {
            FriendId = 1
        };
        var user = _userFakers.User.RuleFor(x => x.Id, 1).Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);
        _mapper.Setup(s => s.Map<FriendRequestDto>(It.IsAny<FriendRequest>())).Returns(dto);

        // Act
        var response = await _friendRequestService.AcceptFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().BeEmpty();
        response.FriendRequest.Should().NotBeNull();

        _friendRequestRepository.Verify(v => v.SaveAllChanges(), Times.Once);
    }

    [Fact]
    public void DeclineFriendRequest_whenFriendRequestInvalid_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;

        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns((FriendRequest?)null);

        // Act
        var response = _friendRequestService.DeclineFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();
    }

    [Fact]
    public void DeclineFriendRequest_whenFriendRequestAlreadyAccepted_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;

        var friendRequest = new FriendRequest
        {
            FriendId = 1,
            IsAccepted = true
        };

        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);

        // Act
        var response = _friendRequestService.DeclineFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();
    }

    [Fact]
    public void DeclineFriendRequest_WhenAuthenticatedUserIsNotTheReceiver_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;
        var friendRequest = new FriendRequest
        {
            FriendId = 1
        };
        var user = _userFakers.User.RuleFor(x => x.Id, 2).Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);

        // Act
        var response = _friendRequestService.DeclineFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.FriendRequest.Should().BeNull();
    }

    [Fact]
    public void DeclineFriendRequest_WhenValid_ShouldReturnResponse()
    {
        // Arrange
        const int searchedId = 1;
        var dto = _friendRequestFaker.FriendRequestDto.Generate();
        var friendRequest = new FriendRequest
        {
            FriendId = 1
        };
        var user = _userFakers.User.RuleFor(x => x.Id, 1).Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(x => x.GetById(searchedId)).Returns(friendRequest);
        _mapper.Setup(s => s.Map<FriendRequestDto>(It.IsAny<FriendRequest>())).Returns(dto);

        // Act
        var response = _friendRequestService.DeclineFriendRequest(searchedId);

        // Assert
        response.Notifications.Should().BeEmpty();
        response.FriendRequest.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFriendRequests_whenExists_ShouldReturnAllFriendRequests()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friendRequests = _friendRequestFaker.FriendRequest.Generate(5);
        var dtos = _friendRequestFaker.FriendRequestDto.Generate(friendRequests.Count);

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);

        _friendRequestRepository.Setup(x => x.GetFriendRequests(It.IsAny<int>())).ReturnsAsync(friendRequests);
        _mapper.Setup(s => s.Map<IEnumerable<FriendRequestDto>>(It.IsAny<IEnumerable<FriendRequest>>())).Returns(dtos);

        // Act
        var response = await _friendRequestService.GetFriendRequests();

        // Assert
        response.Should().BeOfType<List<FriendRequestDto>>();
        response.Count.Should().Be(friendRequests.Count);

        _friendRequestRepository.Verify(v => v.GetFriendRequests(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetFriendsByNameOrEmailEquals_WhenHavingFriends_ShouldReturnFriendsPaginated()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friends = _userFakers.User.Generate(10);
        var friendsDto = _userFakers.UserDto.Generate(friends.Count);


        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetFriendsByFullNameOrEmailEqual(user.Id, "", 1, friends.Count))!
            .ReturnsAsync(friends);

        _mapper.Setup(s => s.Map<List<UserDto>>(friends)).Returns(friendsDto);

        // Act
        var response = await _friendRequestService.GetFriendsByNameOrEmailEquals("", 1, friends.Count);

        // Assert
        response.Should().BeOfType<Pageable<UserDto>>();
        response.Content.Should().NotBeEmpty();
        response.Content.Count.Should().Be(friends.Count);

        _friendRequestRepository.Verify(
            v => v.GetFriendsByFullNameOrEmailEqual(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task RemoveFriend_WhenFriendshipDoesNotExists_ShouldReturnNotifications()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        const int friendshipId = 1;

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetById(friendshipId)).Returns((FriendRequest?)null);

        // Act
        var response = await _friendRequestService.RemoveFriend(friendshipId);

        // Assert
        response.Notifications.Should().NotBeEmpty();

        _friendRequestRepository.Verify(v => v.Remove(It.IsAny<FriendRequest>()), Times.Never);
    }


    [Fact]
    public async Task RemoveFriend_WhenFriendshipIsNotOfCurrentUser_ShouldReturnNotifications()
    {
        // Arrange
        var user = _userFakers.User
            .RuleFor(x => x.Id, 3)
            .Generate();
        const int friendshipId = 1;
        var friendship = _friendRequestFaker.FriendRequest
            .RuleFor(x => x.FriendId, 1)
            .RuleFor(x => x.UserId, 2)
            .Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetById(friendshipId)).Returns(friendship);

        // Act
        var response = await _friendRequestService.RemoveFriend(friendshipId);

        // Assert
        response.Notifications.Should().NotBeEmpty();

        _friendRequestRepository.Verify(v => v.Remove(It.IsAny<FriendRequest>()), Times.Never);
    }

    [Fact]
    public async Task RemoveFriend_WhenFriendshipExists_ShouldNotReturnNotifications()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        const int friendshipId = 1;
        var friendship = _friendRequestFaker.FriendRequest
            .RuleFor(x => x.FriendId, user.Id)
            .Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetById(friendshipId)).Returns(friendship);

        // Act
        var response = await _friendRequestService.RemoveFriend(friendshipId);

        // Assert
        response.Notifications.Should().BeEmpty();

        _friendRequestRepository.Verify(v => v.Remove(It.IsAny<FriendRequest>()), Times.Once);
    }
}