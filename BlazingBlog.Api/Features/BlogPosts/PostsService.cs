using BlazingBlog.Api.Data;
using BlazingBlog.Api.Features.BlogPosts.Exceptions;
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
        try
        {
            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
            return blogPost;
        }
        catch (Exception ex)
        {
            throw new PostNotFoundException($"Post with an id of {id} was not found", ex);
        }
    }

    public async Task<BlogPost> CreateNewPost(string title, string content)
    {
        try
        {
            var newPost = new BlogPost(title, content);

            _context.BlogPosts.Add(newPost);
            await _context.SaveChangesAsync();
            return newPost;
        }
        catch (Exception ex)
        {
            throw new PostNotCreatedException($"Failed to create new post: {title}.", ex);
        }
    }

    public async Task<BlogPost> UpdatePost(int id, string title, string content)
    {
        BlogPost? blogPostToUpdate;

        try
        {
            blogPostToUpdate = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            throw new PostNotFoundException($"Post with an id of {id} was not found", ex);
        }

        try
        {
            blogPostToUpdate.Title = title;
            blogPostToUpdate.Content = content;
            blogPostToUpdate.ModifiedAt.Add(DateTime.UtcNow);

            _context.BlogPosts.Update(blogPostToUpdate);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new PostNotUpdatedException($"Post with an id of {id} was not updated", ex);
        }


        return blogPostToUpdate;
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