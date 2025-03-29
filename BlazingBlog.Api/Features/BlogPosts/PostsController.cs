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
    public async Task<IActionResult> CreatePost([FromBody] string title, [FromBody] string content)
    {
        var newPost = await _service.CreateNewPost(title, content);
        
        if (newPost is null)
            return BadRequest();
        
        return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] string title, [FromBody] string content)
    {
        var updatedPost = await _service.UpdatePost(id, title, content);
        
        return Ok(updatedPost);
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