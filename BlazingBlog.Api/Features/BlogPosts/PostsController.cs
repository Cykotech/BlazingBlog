using BlazingBlog.Api.Features.BlogPosts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BlazingBlog.Api.Features.BlogPosts;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _service;

    public PostsController(IPostService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var posts = await _service.GetAllPosts();
            return Ok(posts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        try
        {
            var post = await _service.GetPostById(id);
            return Ok(post);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] BlogPost newPost)
    {
        try
        {
            var newPostResponse = await _service.CreateNewPost(newPost.Title, newPost.Content);
            return CreatedAtAction(nameof(GetPostById), new { id = newPostResponse.Id }, newPostResponse);
        }
        catch (PostNotCreatedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] BlogPost updatedPost)
    {
        try
        {
            var updatedPostResponse = await _service.UpdatePost(id, updatedPost.Title, updatedPost.Content);

            return Ok(updatedPostResponse);
        }
        catch (PostNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (PostNotUpdatedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        var deletedPost = await _service.DeletePost(id);

        if (!deletedPost)
            return NotFound($"There is no post with id: {id}");

        return NoContent();
    }
}