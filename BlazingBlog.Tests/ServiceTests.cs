using BlazingBlog.Api.Features.BlogPosts;
using Moq;

namespace BlazingBlog.Tests;

public class ServiceTests
{
    private Mock<IPostRepository> _mockPostRepository;
    private PostsService _postService;

    [SetUp]
    public void Setup()
    {
        _mockPostRepository = new Mock<IPostRepository>();
        _postService = new PostsService(_mockPostRepository.Object);
    }

    [Test]
    public async Task Get_All_Posts_Returns_All_Posts()
    {
        var expectedBlogPosts = new List<BlogPost>
        {
            new(title: "Test", content: "Test"),
            new(title: "Test2", content: "Test2"),
            new(title: "Test3", content: "Test3")
        };
        _mockPostRepository.Setup(r => r.GetAllPosts().Result).Returns(expectedBlogPosts);

        var result = await _postService.GetAllPosts();

        Assert.That(result, Is.EqualTo(expectedBlogPosts));
    }

    [Test]
    public async Task Get_Post_By_Id_Returns_Single_Post()
    {
        var listOfBlogPosts = new List<BlogPost>
        {
            new(title: "Test", content: "Test"),
            new(title: "Test2", content: "Test2"),
            new(title: "Test3", content: "Test3")
        };
        var expectedBlogPost = listOfBlogPosts[1];
        _mockPostRepository.Setup(r => r.GetPostById(1).Result).Returns(expectedBlogPost);

        var result = await _postService.GetPostById(1);

        Assert.That(result, Is.EqualTo(expectedBlogPost));
    }

    [Test]
    public async Task Create_Post_Creates_New_Post()
    {
        string newTitle = "Test";
        string newContent = "Test";
        var newBlogPost = new BlogPost(newTitle, newContent);
        _mockPostRepository.Setup(r => r.CreatePost(newBlogPost).Result).Returns(newBlogPost);
        
        var result = await _postService.CreateNewPost(newTitle, newContent);
        
        Assert.That(result.Title, Is.EqualTo(newTitle));
        Assert.That(result.Content, Is.EqualTo(newContent));
    }

    [Test]
    public async Task Update_Post_Updates_Title_And_Content()
    {
        var oldBlogPost = new BlogPost("Test", "Test");
        string updatedTitle = "Test1";
        string updatedContent = "Test1";
        
        _mockPostRepository.Setup(r => r.GetPostById(oldBlogPost.Id).Result).Returns(oldBlogPost);
        _mockPostRepository.Setup(r => r.UpdatePost(oldBlogPost).Result).Returns(oldBlogPost);
        
        var result = await _postService.UpdatePost(oldBlogPost.Id, updatedTitle, updatedContent);
        
        Assert.That(result.Title, Is.EqualTo(updatedTitle));
        Assert.That(result.Content, Is.EqualTo(updatedContent));
    }

    [Test]
    public async Task Delete_Post_Updates_Deleted_At_And_Returns_True_When_Post_Is_Deleted()
    {
        var postToDelete = new BlogPost("Test", "Test");

        _mockPostRepository.Setup(r => r.GetPostById(postToDelete.Id).Result).Returns(postToDelete);
        _mockPostRepository.Setup(r => r.UpdatePost(postToDelete).Result).Returns(postToDelete);
        
        var result = await _postService.DeletePost(postToDelete.Id);
        
        Assert.That(result, Is.EqualTo(true));
        Assert.That(postToDelete.DeletedAt, Is.Not.EqualTo(DateTime.MinValue));
    }
}