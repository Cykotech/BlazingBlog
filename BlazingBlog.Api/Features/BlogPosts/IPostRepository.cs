namespace BlazingBlog.Api.Features.BlogPosts;

public interface IPostRepository
{
    public Task<List<BlogPost>> GetAllPosts();
    public Task<BlogPost> GetPostById(int id);
    public Task<BlogPost> CreatePost(BlogPost post);
    public Task<BlogPost> UpdatePost(BlogPost post);
}