using AutoMapper;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Domain.Models;

namespace YourTale.Application.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<CreatePostRequest, Post>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            ;
    }
}