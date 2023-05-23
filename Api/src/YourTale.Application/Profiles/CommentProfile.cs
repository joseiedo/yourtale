using AutoMapper;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Domain.Models;

namespace YourTale.Application.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            ;
    }
}