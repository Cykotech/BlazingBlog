using BlazingBlog.Api.Data;
using BlazingBlog.Api.Features.BlogPosts.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Api.Features.BlogPosts;

public class PostsRepository : IPostRepository
{
    private readonly BlogDbContext _context;
    
    public PostsRepository(BlogDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<BlogPost>> GetAllPosts()
    {
        return await _context.BlogPosts.ToListAsync();
    }

    public async Task<BlogPost> GetPostById(int id)
    {
        var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);

        if (blogPost is null)
            throw new PostNotFoundException();
        
        return blogPost;
    }

    public async Task<BlogPost> CreatePost(BlogPost post)
    {
        _context.BlogPosts.Add(post);
        int result = await _context.SaveChangesAsync();
        
        if (result == 0)
            throw new PostNotCreatedException();

        return post;
    }

    public async Task<BlogPost> UpdatePost(BlogPost post)
    {
        _context.BlogPosts.Update(post);
        int result = await _context.SaveChangesAsync();
        
        if (result == 0)
            throw new PostNotUpdatedException();

        return post;
    }
}