namespace BlazingBlog.Api.Features.BlogPosts.Exceptions;

public class PostNotFoundException : Exception
{
    public PostNotFoundException()
    {
    }

    public PostNotFoundException(string message)
    {
    }

    public PostNotFoundException(string message, Exception inner)
    {
    }
}