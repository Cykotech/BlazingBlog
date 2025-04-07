using System.Net;
using BlazingBlog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.ApiTests;

public class ControllerTests
{
    private HttpClient _client;
    
    [SetUp]
    public void Setup()
    {
        var factory = new TestApplicationFactory();
        _client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task Get_All_Posts_Returns_200_When_Posts_Exist()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/posts");
        
        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Get_All_Posts_Returns_404_When_Posts_Do_Not_Exist()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/posts");
        
        var response = await _client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}