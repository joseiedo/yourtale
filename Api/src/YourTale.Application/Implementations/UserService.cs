using AutoMapper;
using Microsoft.AspNetCore.Http;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Helpers;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IAddressRepository _addressRepository;


    public UserService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
        IFriendRequestRepository friendRequestRepository, IAddressRepository addressRepository
    )
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _httpContextAccessor = httpContextAccessor;
        _friendRequestRepository = friendRequestRepository;
        _mapper = mapper;
    }

    public User GetAuthenticatedUser()
    {
        var authenticatedUserId =
            _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

        return _userRepository.GetUserById(int.Parse(authenticatedUserId!))!;
    }

    public UserDto GetAuthenticatedUserDetails()
    {
        var user = GetAuthenticatedUser();
        var response = _mapper.Map<UserDto>(user);
        return response;
    }

    public async Task<UserLoginResponse> ValidateLogin(UserLoginRequest request)
    {
        var user = await _userRepository.GetUser(request.Email, request.Password);

        if (user == null)
            return new UserLoginResponse();

        return new UserLoginResponse
            { IsAuthenticated = true, Email = user.Email, Id = user.Id, Role = user.Role };
    }

    public async Task<UserRegisterResponse> RegisterUser(UserRegisterRequest request)
    {
        var response = new UserRegisterResponse();
    
        if (await _userRepository.ExistsByEmail(request.Email))
        {
            response.AddNotification(new Notification("Email já cadastrado"));
            return response;
        }

        var cepData = await _addressRepository.ConsultCep(request.Cep);

        if (cepData is null || !cepData.IsValid)
        {
            response.AddNotification(new Notification("CEP inválido"));
            return response;
        }
        

        var user = _mapper.Map<User>(request);
        user.Password = Hash.Md5Hash(request.Password);
        user.Role = "USER";


        var userEntity = await _userRepository.Add(user);

        response.User = _mapper.Map<UserDto>(userEntity);

        return response;
    }

    public async Task<UserEditResponse> EditUser(UserEditRequest request)
    {
        var response = new UserEditResponse();
        
        var user = _userRepository.GetUserById(request.UserId);
        
        if (user is null)
        {
            response.AddNotification(new Notification("Usuário inválido"));
            return response;
        }
        
        user.NickName = request.Nickname;
        user.Picture = request.Picture;

        await _userRepository.SaveAll();
        
        response.User = _mapper.Map<UserDto>(user);
        
        return response;
    }

    public async Task<GetUserByIdResponse> GetUserById(int id)
    {
        var response = new GetUserByIdResponse();
        var authenticatedUser = GetAuthenticatedUser();
        var user = _userRepository.GetUserById(id);

        if (user is null)
        {
            response.AddNotification(new Notification("Usuário inválido"));
            return response;
        }
        
        var isFriend = await _friendRequestRepository.IsFriend(authenticatedUser.Id, user.Id);
        var addressData = await _addressRepository.ConsultCep(user.Cep);
        
        if (addressData is null)
        {
            response.AddNotification(new Notification("Usuário inválido"));
            return response;
        }
        
        response.IsFriend = isFriend; 
        response.IsLoggedUser = user.Id == authenticatedUser.Id;
        response.User = _mapper.Map<UserDto>(user);
        response.City = addressData.City;
        response.Uf = addressData.Uf;
        
        return response;
    }
    
    
}