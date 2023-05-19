using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;

namespace WebApplication1.Controllers;

[Route("v1/posts")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var response = await _postService.CreatePost(request);
        if (!response.IsValid())
        {
            return BadRequest(new ErrorResponse(response.Notifications));    
        }
        
        return CreatedAtAction(nameof(CreatePost), response);
    }
    
    [HttpPost("{postId:int}/like")]
    [Authorize]
    public async Task<IActionResult> LikePost([FromRoute] int postId)
    {
        await _postService.LikePost(postId);
        return Ok();
    }
    
    [HttpDelete("{postId:int}/unlike")]
    [Authorize]
    public IActionResult UnlikePost([FromRoute] int postId)
    {
        _postService.UnlikePost(postId);
        return Ok();
    }
     
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int take = 6
        )
    {
        return Ok(await _postService.GetPosts(page, take));
    }
    
    [HttpGet("{userId:int}")]
    [Authorize]
    public async Task<IActionResult> GetByUserId(
        [FromRoute] int userId,
        [FromQuery] int page = 1,
        [FromQuery] int take = 6
    )
    {
        return Ok(await _postService.GetPostsByUserId(userId, page, take));
    }
}