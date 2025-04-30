using Microsoft.EntityFrameworkCore;
using BlazingBlog.Api.Features.BlogPosts;

namespace BlazingBlog.Api.Data;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts { get; set; }
}