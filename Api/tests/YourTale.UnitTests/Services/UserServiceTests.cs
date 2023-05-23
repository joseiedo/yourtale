using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Implementations;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.UnitTests.Fakers;

namespace YourTale.UnitTests.Services;

public class UserServiceTests
{
    private readonly AddressFakers _addressFakers = new();
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly Mock<IFriendRequestRepository> _friendRequestRepository = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly UserFakers _userFakers = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(_mapper.Object, _httpContextAccessor.Object,
            _userRepository.Object, _friendRequestRepository.Object, _addressRepository.Object);
    }

    [Fact]
    public async Task ValidateLogin_WhenUserExists_ReturnsUserLoginResponse()
    {
        // Arrange
        var user = _userFakers.User.Generate();

        _userRepository.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

        // Act
        var result = await _userService.ValidateLogin(new UserLoginRequest());

        // Assert
        Assert.IsType<UserLoginResponse>(result);
        result.Should().BeOfType<UserLoginResponse>();
        result.IsAuthenticated.Should().BeTrue();
        result.Email.Should().Be(user.Email);
        result.Id.Should().Be(user.Id);
        result.Role.Should().Be(user.Role);
    }

    [Fact]
    public async Task ValidateLogin_WhenUserDoesNotExist_ReturnsNullUserLoginResponse()
    {
        // Arrange
        _userRepository.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.ValidateLogin(new UserLoginRequest());

        // Assert
        result.Should().BeOfType<UserLoginResponse>();
        result.IsAuthenticated.Should().BeFalse();
        result.Email.Should().BeNull();
        result.Id.Should().Be(0);
        result.Role.Should().BeNull();
    }


    [Fact]
    public async Task RegisterUser_WhenEmailExists_ShouldNotRegister()
    {
        // Arrange
        var userRegisterRequest = _userFakers.UserRegisterRequest.Generate();

        _userRepository.Setup(s => s.ExistsByEmail(userRegisterRequest.Email)).ReturnsAsync(true);

        // Act
        var response = await _userService.RegisterUser(userRegisterRequest);

        // Assert
        response.Should().BeOfType<UserRegisterResponse>();
        response.Notifications.Should().NotBeEmpty();
        response.User.Should().BeNull();

        _userRepository.Verify(x => x.ExistsByEmail(It.IsAny<string>()), Times.Once);
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUser_WhenCepIsInvalid_ShouldNotRegister()
    {
        // Arrange
        var userRegisterRequest = _userFakers.UserRegisterRequest.Generate();
        var invalidCep = new Address();

        _userRepository.Setup(s => s.ExistsByEmail(userRegisterRequest.Email)).ReturnsAsync(false);
        _addressRepository.Setup(s => s.ConsultCep(userRegisterRequest.Cep)).ReturnsAsync(invalidCep);

        // Act
        var response = await _userService.RegisterUser(userRegisterRequest);

        // Assert
        response.Should().BeOfType<UserRegisterResponse>();
        response.Notifications.Should().NotBeEmpty();
        response.User.Should().BeNull();

        _addressRepository.Verify(x => x.ConsultCep(It.IsAny<string>()), Times.Once);
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUser_WhenValid_ShouldRegister()
    {
        // Arrange
        var userRegisterRequest = _userFakers.UserRegisterRequest.Generate();
        var validAddres = _addressFakers.Address.Generate();
        var user = _userFakers.User.Generate();
        var dto = _userFakers.UserDto.Generate();

        _userRepository.Setup(s => s.ExistsByEmail(userRegisterRequest.Email)).ReturnsAsync(false);
        _addressRepository.Setup(s => s.ConsultCep(userRegisterRequest.Cep)).ReturnsAsync(validAddres);
        _mapper.Setup(s => s.Map<User>(It.IsAny<UserRegisterRequest>())).Returns(user);
        _mapper.Setup(s => s.Map<UserDto>(It.IsAny<User>())).Returns(dto);


        // Act
        var response = await _userService.RegisterUser(userRegisterRequest);


        // Assert
        response.Should().BeOfType<UserRegisterResponse>();
        response.Notifications.Should().BeEmpty();
        response.User.Should().BeOfType<UserDto>();
        response.User.Should().NotBeNull();

        _userRepository.Verify(v => v.Add(user), Times.Once);
    }

    [Fact]
    public async Task EditUser_WhenUserDontExists_ShouldNotEdit()
    {
        // Arrange
        var request = _userFakers.UserEditRequest.Generate();
        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns((User?)null);

        // Act
        var response = await _userService.EditUser(request);

        // Assert
        response.Should().BeOfType<UserEditResponse>();
        response.Notifications.Should().NotBeEmpty();
        response.User.Should().BeNull();

        _userRepository.Verify(v => v.GetUserById(request.UserId), Times.Once);
        _userRepository.Verify(v => v.SaveAll(), Times.Never);
    }


    [Fact]
    public async Task EditUser_WhenUserExists_ShouldEdit()
    {
        // Arrange
        var request = _userFakers.UserEditRequest.Generate();
        var user = _userFakers.User.Generate();
        var dto = _userFakers.UserDto.Generate();

        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns(user);
        _mapper.Setup(s => s.Map<UserDto>(user)).Returns(dto);

        // Act
        var response = await _userService.EditUser(request);

        // Assert
        response.Should().BeOfType<UserEditResponse>();
        response.Notifications.Should().BeEmpty();
        response.User.Should().NotBeNull();

        _userRepository.Verify(v => v.SaveAll(), Times.Once);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user1 = _userFakers.User.RuleFor(x => x.Id, 1).Generate();
        var user2 = _userFakers.User.RuleFor(x => x.Id, 2).Generate();
        var address = _addressFakers.Address.Generate();
        var dto = _userFakers.UserDto.Generate();
        var friendship = new FriendRequest
        {
            Id = 1,
            User = user1,
            Friend = user2,
            UserId = user1.Id,
            FriendId = user2.Id,
        };

        _httpContextAccessor.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", user1.Id.ToString()),
            }, "mock"))
        });

        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns(user1);
        _addressRepository.Setup(s => s.ConsultCep(It.IsAny<string>())).ReturnsAsync(address);
        _friendRequestRepository.Setup(s => s.IsFriend(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
        _friendRequestRepository.Setup(s => s.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(friendship);
        _mapper.Setup(s => s.Map<UserDto>(user1)).Returns(dto);

        // Act
        var response = await _userService.GetUserById(It.IsAny<int>());

        // Assert
        response.User.Should().NotBeNull();
        _userRepository.Verify(v => v.GetUserById(It.IsAny<int>()), Times.AtLeast(1));
    }

    [Fact]
    public async Task GetUserById_WhenuserDontExists_ShouldReturnNull()
    {
        // Arrange
        _httpContextAccessor.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", "1"),
            }, "mock"))
        });
        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns((User?)null);

        // Act
        var response = await _userService.GetUserById(It.IsAny<int>());

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.User.Should().BeNull();
        _userRepository.Verify(v => v.GetUserById(It.IsAny<int>()), Times.AtLeast(1));
    }

    [Fact]
    public async Task GetUserById_WhenAddresIsInvalid_ShouldReturnNull()
    {

        // Arrange
        var user = _userFakers.User.Generate();
        _httpContextAccessor.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
            }, "mock"))
        });
        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns((User?)user);
        _addressRepository.Setup(s => s.ConsultCep(It.IsAny<string>())).ReturnsAsync((Address?)null);

        // Act
        var response = await _userService.GetUserById(It.IsAny<int>());

        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.User.Should().BeNull();
        _userRepository.Verify(v => v.GetUserById(It.IsAny<int>()), Times.AtLeast(1));
        _addressRepository.Verify(v => v.ConsultCep(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetUsersByNameOrEmailEquals_WhenHavingUsers_ShouldReturnUsersPaginated()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friends = _userFakers.User.Generate(10);
        var friendsDto = _userFakers.UserDto.Generate(friends.Count);

        _httpContextAccessor.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
            }, "mock"))
        });

        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns((User?)user);
        _userRepository.Setup(s => s.GetUsersByFullNameOrEmailEqual(user.Id, "", 1, friends.Count))
            .ReturnsAsync(friends);

        _mapper.Setup(s => s.Map<List<UserDto>>(friends)).Returns(friendsDto);

        // Act
        var response = await _userService.GetUsersByNameOrEmailEquals("", 1, friends.Count);

        // Assert
        response.Should().BeOfType<Pageable<UserDto>>();
        response.Content.Should().NotBeEmpty();
        response.Content.Count.Should().Be(friends.Count);

        _userRepository.Verify(v => v.GetUserById(It.IsAny<int>()), Times.Once);
        _userRepository.Verify(
            v => v.GetUsersByFullNameOrEmailEqual(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void GetAuthenticatedUserDetails_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var dto = _userFakers.UserDto.Generate();

        _httpContextAccessor.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
            }, "mock"))
        });

        _userRepository.Setup(s => s.GetUserById(It.IsAny<int>())).Returns(user);
        _mapper.Setup(s => s.Map<UserDto>(user)).Returns(dto);

        // Act
        var response = _userService.GetAuthenticatedUserDetails();

        // Assert
        response.Should().BeOfType<UserDto>();
        response.Should().NotBeNull();

        _userRepository.Verify(v => v.GetUserById(It.IsAny<int>()), Times.Once);
    }
    

}
