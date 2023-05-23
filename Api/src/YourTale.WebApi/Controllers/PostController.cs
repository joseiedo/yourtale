using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;

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
        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return CreatedAtAction(nameof(CreatePost), response.Post);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditPost([FromBody] EditPostRequest request)
    {
        var response = await _postService.EditPost(request);
        if (!response.IsValid()) return NotFound(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpPost("{postId:int}/like")]
    [Authorize]
    public async Task<IActionResult> LikePost([FromRoute] int postId)
    {
        var response = await _postService.LikePost(postId);
        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpDelete("{postId:int}/unlike")]
    [Authorize]
    public async Task<IActionResult> UnlikePost([FromRoute] int postId)
    {
        var response = await _postService.UnlikePost(postId);

        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpGet("details/{postId:int}")]
    [Authorize]
    public IActionResult GetPostDetails([FromRoute] int postId)
    {
        var response = _postService.GetPostDetails(postId);

        if (!response.IsValid()) return NotFound(new ErrorResponse(response.Notifications));

        return Ok(response);
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

    [HttpPost("comments")]
    [Authorize]
    public async Task<IActionResult> CommentPost([FromBody] CommentPostRequest request)
    {
        await _postService.CommentPost(request);
        return NoContent();
    }
}