using AutoMapper;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Domain.Models;

namespace YourTale.Application.Profiles;

public class UserProfile : Profile
{

    public UserProfile()
    {
        
        CreateMap<UserRegisterRequest, User>();

    }
    
}