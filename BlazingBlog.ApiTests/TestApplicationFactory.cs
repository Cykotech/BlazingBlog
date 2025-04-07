using Microsoft.AspNetCore.Mvc.Testing;
using BlazingBlog.Api;
using Microsoft.AspNetCore.Hosting;

namespace BlazingBlog.ApiTests;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        base.ConfigureWebHost(builder);
    }
}