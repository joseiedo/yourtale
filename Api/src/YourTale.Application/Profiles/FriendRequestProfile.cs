using AutoMapper;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Domain.Models;

namespace YourTale.Application.Profiles;

public class FriendRequestProfile : Profile
{

    public FriendRequestProfile()
    {
        CreateMap<FriendRequest, FriendRequestDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }
}