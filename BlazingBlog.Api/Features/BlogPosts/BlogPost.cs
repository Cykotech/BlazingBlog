using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Api.Features.BlogPosts;

// Database Entity
public class BlogPost
{
    public int Id { get; init; }

    [Required] public string Title { get; set; }

    [Required] public string Content { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public List<DateTime> ModifiedAt { get; } = [];
    
    public DateTime DeletedAt { get; set; }

    public BlogPost(string title, string content)
    {
        Title = title;
        Content = content;
    }
}