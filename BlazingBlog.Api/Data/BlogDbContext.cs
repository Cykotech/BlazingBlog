using BlazingBlog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Api.Data;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts { get; set; }
}