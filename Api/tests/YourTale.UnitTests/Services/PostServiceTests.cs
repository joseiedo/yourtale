using AutoMapper;
using FluentAssertions;
using Moq;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Application.Implementations;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.UnitTests.Fakers;

namespace YourTale.UnitTests.Services;

public class PostServiceTests
{
    
    private readonly Mock<IFriendRequestRepository> _friendRequestRepository = new();
    private readonly Mock<ILikeRepository> _likeRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IPostRepository> _postRepository = new();
    private readonly Mock<IUserService> _userService = new();
    private readonly IPostService _postService; 
    private readonly PostFakers _postFakers = new();
    private readonly UserFakers _userFakers = new();

    public PostServiceTests()
    {
        _postService = new PostService(
            _mapper.Object,
            _postRepository.Object,
            _friendRequestRepository.Object,
            _likeRepository.Object,
            _userService.Object
        );
    }


    [Fact]
    public async Task CreatePost_WhenValid_ShouldCreatePost()
    {
        
        // Arrange
        var user = _userFakers.User.Generate();
        var post = _postFakers.Post.Generate();
        var dto = _postFakers.PostDto.Generate();
        
        
        _userService.Setup(x => x.GetAuthenticatedUser()).Returns(user);
        _mapper.Setup(x => x.Map<Post>(It.IsAny<CreatePostRequest>())).Returns(post);
        _mapper.Setup(x => x.Map<PostDto>(It.IsAny<Post>())).Returns(dto);
        _postRepository.Setup(x => x.Add(It.IsAny<Post>())).ReturnsAsync(post); 
        
        // Act 
        var response = await _postService.CreatePost(_postFakers.CreatePostRequest.Generate());
        
        // Assert
        response.Notifications.Should().BeEmpty();
        response.Post.Should().BeEquivalentTo(dto);
        _postRepository.Verify(v => v.Add(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public void GetPostDetails_WhenPostDoesntExists_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;
        _postRepository.Setup(s => s.GetById(searchedId)).Returns((Post?) null);
       
       
        // Act
        var response = _postService.GetPostDetails(searchedId);
         
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
    }

    [Fact]
    public void  GetPostDetails_WhenPostExists_ShouldReturnPost()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var user = _userFakers.User.Generate();
        const int searchedId = 1;


        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _postRepository.Setup(s => s.GetById(searchedId)).Returns(post);
        _mapper.Setup(x => x.Map<PostDto>(It.IsAny<Post>())).Returns(_postFakers.PostDto.Generate());
        _mapper.Setup(x => x.Map<List<CommentDto>>(It.IsAny<List<Comment>>())).Returns(new List<CommentDto>());
        _likeRepository.Setup(s => s.IsLiked(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
        
        // Act
        var response = _postService.GetPostDetails(searchedId);
        
        // Assert
        response.Notifications.Should().BeEmpty();
        response.Post.Should().NotBeNull();
    }

    [Fact]
    public async Task EditPost_WhenPostDoesntExists_ShouldReturnNotification()
    {
        // Arrange
        const int searchedId = 1;
        var request = new EditPostRequest
        {
            IsPrivate = true,
            PostId = searchedId 
        };

        _postRepository.Setup(s => s.GetById(searchedId)).Returns((Post?) null);
           
           
        // Act
        var response = await _postService.EditPost(request);
             
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
        _postRepository.Verify(v => v.SaveAll(), Times.Never);
    }
    
    [Fact]
    public async Task EditPost_WhenPostExists_ShouldReturnPost()
    {
        // Arrange
        const int searchedId = 1;
        var post = _postFakers.Post.Generate();
        var dto = _postFakers.PostDto.Generate();
        var request = new EditPostRequest
        {
            IsPrivate = true,
            PostId = searchedId 
        };
    
        _postRepository.Setup(s => s.GetById(searchedId)).Returns(post);
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(dto);
               
        // Act
        var response = await _postService.EditPost(request);
                 
        // Assert
        response.Notifications.Should().BeEmpty();
        response.Post.Should().NotBeNull();
        _postRepository.Verify(v => v.SaveAll(), Times.Once);
    }

    [Fact]
    public void CommentPost_WhenPostDontExist_ShouldNotAddComment()
    {
        // Arrange
        var request = new CommentPostRequest();
        
        _postRepository.Setup(s => s.GetById(request.PostId)).Returns((Post?) null);
        
        // Act
        _postService.CommentPost(request);
        
        // Assert
        _postRepository.Verify(v => v.SaveAll(), Times.Never);
    }
    
    
    [Fact]
    public void CommentPost_WhenPostExist_ShouldAddComment()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var request = new CommentPostRequest
        {
            PostId = post.Id,
            Text = "Test"
        };
        
        _postRepository.Setup(s => s.GetById(request.PostId)).Returns(post);

        // Act
        _postService.CommentPost(request);
        
        // Assert
        _postRepository.Verify(v => v.SaveAll(), Times.Once);
    }
    
    [Fact]
    public async Task LikePost_WhenPostDontExist_ShouldNotLikePost()
    {
        // Arrange
        const int postId = 1;
        
        _postRepository.Setup(s => s.GetById(postId)).Returns((Post?) null);
        
        // Act
        var response  = await _postService.LikePost(postId);
        
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
        _likeRepository.Verify(v => v.Add(It.IsAny<Like>()), Times.Never);
    }

    [Fact]
    public async Task LikePost_WhenAlreadyLiked_ShouldNotLikePost()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var user = _userFakers.User.Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _postRepository.Setup(s => s.GetById(post.Id)).Returns(post);
        _likeRepository.Setup(s => s.IsLiked(user.Id, post.Id)).Returns(true);
        
        // Act
        var response = await _postService.LikePost(post.Id);
        
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
        _likeRepository.Verify(v => v.Add(It.IsAny<Like>()), Times.Never);
    }

    [Fact]
    public async Task LikePost_WhenValid_ShouldLikePost()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var user = _userFakers.User.Generate();
        
        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _postRepository.Setup(s => s.GetById(post.Id)).Returns(post);
        _likeRepository.Setup(s => s.IsLiked(user.Id, post.Id)).Returns(false);
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(_postFakers.PostDto.Generate());
                
        // Act
        var response = await _postService.LikePost(post.Id);
                
        // Assert
        response.Notifications.Should().BeEmpty();
        response.Post.Should().NotBeNull();
        _likeRepository.Verify(v => v.Add(It.IsAny<Like>()), Times.Once);
    }
    
    [Fact]
    public async Task UnlikePost_WhenPostDontExist_ShouldNotUnlikePost()
    {
        // Arrange
        const int postId = 1;
        
        _postRepository.Setup(s => s.GetById(postId)).Returns((Post?) null);
        
        // Act
        var response  = await _postService.UnlikePost(postId);
        
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
        _likeRepository.Verify(v => v.RemoveById(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task UnlikePost_WhenNotLiked_ShouldNotUnlikePost()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var user = _userFakers.User.Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _postRepository.Setup(s => s.GetById(post.Id)).Returns(post);
        _likeRepository.Setup(s => s.IsLiked(user.Id, post.Id)).Returns(false);
        
        // Act
        var response = await _postService.UnlikePost(post.Id);
        
        // Assert
        response.Notifications.Should().NotBeEmpty();
        response.Post.Should().BeNull();
        _likeRepository.Verify(v => v.RemoveById(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task UnlikePost_WhenValid_ShouldUnlikePost()
    {
        // Arrange
        var post = _postFakers.Post.Generate();
        var user = _userFakers.User.Generate();
        
        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _postRepository.Setup(s => s.GetById(post.Id)).Returns(post);
        _likeRepository.Setup(s => s.IsLiked(user.Id, post.Id)).Returns(true);
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(_postFakers.PostDto.Generate());
                
        // Act
        var response = await _postService.UnlikePost(post.Id);
                
        // Assert
        response.Notifications.Should().BeEmpty();
        response.Post.Should().NotBeNull();
        _likeRepository.Verify(v => v.RemoveById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetPosts_WhenExists_ShouldReturnPostsPaginated()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friends = _userFakers.User.Generate(10);

        var posts = _postFakers.Post.Generate(10);
        var postDto = _postFakers.PostDto.Generate();

        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetFriends(user.Id)).Returns(friends!);
        _postRepository.Setup(s => s.GetPosts(friends!, user.Id, 1, posts.Count)).ReturnsAsync(posts);
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(postDto);
        _likeRepository.Setup(s => s.IsLiked(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
        
        // Act
        var response = await _postService.GetPosts( 1, posts.Count);
       
        // Assert
        response.Should().BeOfType<Pageable<PostDto>>();
        response.Content.Should().NotBeEmpty();
        response.Content.Should().HaveCount(posts.Count);
    }
    
    [Fact]
    public async Task GetPostsByUserId_WhenExistsAndIsFromCurrentUser_ShouldReturnPostsPaginated()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friends = _userFakers.User.Generate(10);
    
        var posts = _postFakers.Post.Generate(10);
        var postDto = _postFakers.PostDto.Generate();
    
        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetFriends(user.Id)).Returns(friends!);
        
        _postRepository.Setup(s => s.
            GetPostsByUserId(It.IsAny<bool>(), user.Id, 1, posts.Count)).ReturnsAsync(posts);
        
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(postDto);
        _likeRepository.Setup(s => s.IsLiked(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            
        // Act
        var response = await _postService.GetPostsByUserId(user.Id, 1, posts.Count);
           
        // Assert
        response.Should().BeOfType<Pageable<PostDto>>();
        response.Content.Should().NotBeEmpty();
        response.Content.Should().HaveCount(posts.Count);
    }
    
     
    [Fact]
    public async Task GetPostsByUserId_WhenExistsAndIsFromFriend_ShouldReturnPostsPaginated()
    {
        // Arrange
        var user = _userFakers.User.Generate();
        var friends = _userFakers.User.Generate(1);
    
        var posts = _postFakers.Post.Generate(10);
        var postDto = _postFakers.PostDto.Generate();
    
        _userService.Setup(s => s.GetAuthenticatedUser()).Returns(user);
        _friendRequestRepository.Setup(s => s.GetFriends(user.Id)).Returns(friends!);
        
        _postRepository.Setup(s => s.
            GetPostsByUserId(It.IsAny<bool>(), friends[0].Id, 1, posts.Count)).ReturnsAsync(posts);
        
        _mapper.Setup(s => s.Map<PostDto>(It.IsAny<Post>())).Returns(postDto);
        _likeRepository.Setup(s => s.IsLiked(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            
        // Act
        var response = await _postService.GetPostsByUserId(friends[0].Id, 1, posts.Count);
           
        // Assert
        response.Should().BeOfType<Pageable<PostDto>>();
        response.Content.Should().NotBeEmpty();
        response.Content.Should().HaveCount(posts.Count);
    }
}