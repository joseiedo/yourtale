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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var response = await _postService.CreatePost(request);
        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return CreatedAtAction(nameof(CreatePost), response.Post);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> EditPost([FromBody] EditPostRequest request)
    {
        var response = await _postService.EditPost(request);
        if (!response.IsValid()) return NotFound(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpPost]
    [Route("{postId:int}/like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> LikePost([FromRoute] int postId)
    {
        var response = await _postService.LikePost(postId);
        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpDelete]
    [Route("{postId:int}/unlike")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> UnlikePost([FromRoute] int postId)
    {
        var response = await _postService.UnlikePost(postId);

        if (!response.IsValid()) return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }

    [HttpGet]
    [Route("details/{postId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPostDetailsResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public IActionResult GetPostDetails([FromRoute] int postId)
    {
        var response = _postService.GetPostDetails(postId);

        if (!response.IsValid()) return NotFound(new ErrorResponse(response.Notifications));

        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pageable<PostDto>))]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int take = 6
    )
    {
        return Ok(await _postService.GetPosts(page, take));
    }

    [HttpGet]
    [Route("{userId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pageable<PostDto>))]
    public async Task<IActionResult> GetByUserId(
        [FromRoute] int userId,
        [FromQuery] int page = 1,
        [FromQuery] int take = 6
    )
    {
        return Ok(await _postService.GetPostsByUserId(userId, page, take));
    }

    [HttpPost]
    [Route("comments")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CommentPost([FromBody] CommentPostRequest request)
    {
        await _postService.CommentPost(request);
        return NoContent();
    }
}