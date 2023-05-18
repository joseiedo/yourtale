using AutoMapper;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Models;

namespace YourTale.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterRequest, User>();
        CreateMap<User, UserDto>();
    }
}