using BlazingBlog.Api.Models;

namespace BlazingBlog.Api.Interfaces;

public interface IPostService
{
    public Task<List<BlogPost>> GetAllPosts();
    public Task<BlogPost> GetPostById(int id);
    public Task<BlogPost> CreateNewPost(string title, string content);
    public Task<BlogPost> UpdatePost(int id, string title, string content);
    public Task<bool> DeletePost(int id);
}