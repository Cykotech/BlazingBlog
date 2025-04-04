using BlazingBlog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazingBlog.Api.Features.BlogPosts;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private PostsService _service;

    public PostsController(PostsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _service.GetAllPosts();
        
        if (posts is null)
            return NotFound();
        
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        var post = await _service.GetPostById(id);
        
        if (post is null)
            return NotFound();
        
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] BlogPost newPost)
    {
        var newPostResponse = await _service.CreateNewPost(newPost.Title, newPost.Content);
        
        if (newPostResponse is null)
            return BadRequest();
        
        return CreatedAtAction(nameof(GetPostById), new { id = newPostResponse.Id }, newPostResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] BlogPost updatedPost)
    {
        var updatedPostResponse = await _service.UpdatePost(id, updatedPost.Title, updatedPost.Content);
        
        if (updatedPostResponse is null)
            return BadRequest();
        
        return Ok(updatedPostResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        var deletedPost = await _service.DeletePost(id);
        
        if (!deletedPost)
            return NotFound();
        
        return NoContent();
    }
}