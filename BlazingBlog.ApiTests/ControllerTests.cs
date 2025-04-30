using BlazingBlog.Api.Features.BlogPosts;
using BlazingBlog.Api.Features.BlogPosts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlazingBlog.ApiTests;

public class ControllerTests
{
    private Mock<IPostService> _mockPostsService;
    private PostsController _postsController;

    [SetUp]
    public void Setup()
    {
        _mockPostsService = new Mock<IPostService>();
        _postsController = new PostsController(_mockPostsService.Object);
    }

    [Test]
    public async Task Get_All_Posts_Returns_200_When_Posts_Exist()
    {
        var expectedBlogPosts = new List<BlogPost>();
        expectedBlogPosts.Add(new(title: "Test", content: "Test"));
        expectedBlogPosts.Add(new(title: "Test2", content: "Test2"));
        expectedBlogPosts.Add(new(title: "Test3", content: "Test3"));
        _mockPostsService.Setup(s => s.GetAllPosts().Result).Returns(expectedBlogPosts);

        var result = await _postsController.GetAllPosts() as OkObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
        Assert.That(result?.Value, Is.EqualTo(expectedBlogPosts));
    }

    [Test]
    public async Task Get_All_Posts_Returns_500_When_Posts_Cannot_Be_Retrieved()
    {
        _mockPostsService.Setup(s => s.GetAllPosts().Result).Throws<InvalidOperationException>();

        var result = await _postsController.GetAllPosts() as ObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(500));
        Assert.That(result?.Value, Is.EqualTo("Operation is not valid due to the current state of the object."));
    }

    [Test]
    public async Task Get_Post_By_Id_Returns_200_When_Post_Exists()
    {
        var expectedBlogPost = new BlogPost(title: "Test", content: "Test");
        _mockPostsService.Setup(s => s.GetPostById(expectedBlogPost.Id).Result).Returns(expectedBlogPost);

        var result = await _postsController.GetPostById(expectedBlogPost.Id) as OkObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
        Assert.That(result?.Value, Is.EqualTo(expectedBlogPost));
    }

    [Test]
    public async Task Get_Post_By_Id_Returns_404_When_Post_Does_Not_Exist()
    {
        _mockPostsService.Setup(s => s.GetPostById(88).Result).Throws<PostNotFoundException>();

        var result = await _postsController.GetPostById(88) as NotFoundObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(404));
        Assert.That(result?.Value, Is.EqualTo($"Exception of type \'{typeof(PostNotFoundException)}\' was thrown."));
    }

    [Test]
    public async Task Create_Post_Returns_201_When_Post_Is_Created()
    {
        var newBlogPost = new BlogPost(title: "Test", content: "Test");
        _mockPostsService.Setup(s => s.CreateNewPost(newBlogPost.Title, newBlogPost.Content).Result)
            .Returns(newBlogPost);

        var result = await _postsController.CreatePost(newBlogPost) as CreatedAtActionResult;

        Assert.That(result?.StatusCode, Is.EqualTo(201));
        Assert.That(result?.Value, Is.EqualTo(newBlogPost));
    }

    [Test]
    public async Task Create_Post_Returns_400_When_Post_Is_Not_Created()
    {
        BlogPost badBlogPost = new("Bad Post", "No Content");
        _mockPostsService.Setup(s => s.CreateNewPost(badBlogPost.Title, badBlogPost.Content).Result)
            .Throws<PostNotCreatedException>();

        var result = await _postsController.CreatePost(badBlogPost) as BadRequestObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_Post_Returns_200_When_Post_Is_Updated()
    {
        BlogPost updatedBlogPost = new("New Title", "Updated Content");
        _mockPostsService.Setup(s => s.UpdatePost(1, updatedBlogPost.Title, updatedBlogPost.Content).Result)
            .Returns(updatedBlogPost);

        var result = await _postsController.UpdatePost(1, updatedBlogPost) as OkObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(200));
        Assert.That(result?.Value, Is.EqualTo(updatedBlogPost));
    }

    [Test]
    public async Task Update_Post_Returns_404_When_Post_Is_Not_Found()
    {
        BlogPost invalidBlogPost = new("New Title", "Invalid Content");
        _mockPostsService.Setup(s => s.UpdatePost(1, invalidBlogPost.Title, invalidBlogPost.Content).Result)
            .Throws<PostNotFoundException>();

        var result = await _postsController.UpdatePost(1, invalidBlogPost) as NotFoundObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(404));
        Assert.That(result?.Value, Is.EqualTo($"Exception of type \'{typeof(PostNotFoundException)}\' was thrown."));
    }
    
    [Test]
    public async Task Update_Post_Returns_400_When_Post_Is_Not_Updated()
    {
        BlogPost invalidBlogPost = new("New Title", "Invalid Content");
        _mockPostsService.Setup(s => s.UpdatePost(1, invalidBlogPost.Title, invalidBlogPost.Content).Result)
            .Throws<PostNotUpdatedException>();

        var result = await _postsController.UpdatePost(1, invalidBlogPost) as BadRequestObjectResult;

        Assert.That(result?.StatusCode, Is.EqualTo(400));
        Assert.That(result?.Value, Is.EqualTo($"Exception of type \'{typeof(PostNotUpdatedException)}\' was thrown."));
    }

    [Test]
    public async Task Delete_Post_Returns_204_When_Post_Is_Deleted()
    {
        int id = 1;
        _mockPostsService.Setup(s => s.DeletePost(id).Result).Returns(true);
        
        var result = await _postsController.DeletePost(id) as NoContentResult;
        
        Assert.That(result?.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public async Task Delete_Post_Returns_404_When_Post_Is_Not_Deleted()
    {
        int id = 1;
        _mockPostsService.Setup(s => s.DeletePost(id).Result).Returns(false);
        
        var result = await _postsController.DeletePost(id) as NotFoundObjectResult;
        
        Assert.That(result?.StatusCode, Is.EqualTo(404));
        Assert.That(result?.Value, Is.EqualTo($"There is no post with id: {id}"));
    }
}