using BlazingBlog.Api.Data;
using BlazingBlog.Api.Interfaces;
using BlazingBlog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Api.Features.BlogPosts;

public class PostsService : IPostService
{
    private readonly BlogDbContext _context;

    public PostsService(BlogDbContext context)
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
            return null;

        return blogPost;
    }

    public async Task<BlogPost> CreateNewPost(string title, string content)
    {
        var newPost = new BlogPost(title, content);
        
        _context.BlogPosts.Add(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }

    public async Task<BlogPost> UpdatePost(int id, string title, string content)
    {
        var blogPost = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
        
        if (blogPost is null)
            return null;
        
        blogPost.Title = title;
        blogPost.Content = content;
        blogPost.ModifiedAt.Add(DateTime.UtcNow);
        
        _context.BlogPosts.Update(blogPost);
        await _context.SaveChangesAsync();

        return blogPost;
    }

    public async Task<bool> DeletePost(int id)
    {
            var blogPost = _context.BlogPosts.FirstOrDefault(p => p.Id == id);

            if (blogPost is null)
                return false;
            
            blogPost.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return true;
    }
}