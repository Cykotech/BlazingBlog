using BlazingBlog.Api.Features.BlogPosts.Exceptions;

namespace BlazingBlog.Api.Features.BlogPosts;

public class PostsService : IPostService
{
    private readonly IPostRepository _repository;

    public PostsService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BlogPost>> GetAllPosts()
    {
        return await _repository.GetAllPosts();
    }

    public async Task<BlogPost> GetPostById(int id)
    {
        try
        {
            return await _repository.GetPostById(id);
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
            await _repository.CreatePost(newPost);
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
            blogPostToUpdate = await _repository.GetPostById(id);

            blogPostToUpdate.Title = title;
            blogPostToUpdate.Content = content;
            blogPostToUpdate.ModifiedAt.Add(DateTime.UtcNow);

            await _repository.UpdatePost(blogPostToUpdate);
        }
        catch (PostNotFoundException ex)
        {
            throw new PostNotFoundException($"Post with an id of {id} was not found", ex);
        }
        catch (PostNotUpdatedException ex)
        {
            throw new PostNotUpdatedException($"Post with an id of {id} was not updated", ex);
        }


        return blogPostToUpdate;
    }

    public async Task<bool> DeletePost(int id)
    {
        BlogPost blogPostToDelete;
        try
        {
            blogPostToDelete = await _repository.GetPostById(id);
        }
        catch (Exception ex)
        {
            throw new PostNotFoundException($"Post with an id of {id} was not found", ex);
        }

        try
        {
            blogPostToDelete.DeletedAt = DateTime.UtcNow;
            await _repository.UpdatePost(blogPostToDelete);
        }
        catch (Exception ex)
        {
            throw new PostNotUpdatedException($"Post with an id of {id} was not deleted", ex);
        }

        return true;
    }
}