using Domain.ToDoItems;
using Flurl.Http;
using Responses.Http;
using WebApi;

namespace UnitTest.IntegratedTests;

public class WorkFlowIntegratedTest : AbstractIntegratedTest
{
    public WorkFlowIntegratedTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll()
    {
        var t = await CallHttp("/Todo").GetAsync().ReceiveResult<List<ToDoItemEntity>>();

        Assert.True(t.IsSuccess);
        Assert.NotEmpty(t.Value);
    }
    
}