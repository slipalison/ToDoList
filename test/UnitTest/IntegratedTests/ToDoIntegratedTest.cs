using Domain.Commands;
using Domain.ToDoItems;
using Flurl.Http;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Responses.Http;
using WebApi;

namespace UnitTest.IntegratedTests;

public class WorkFlowIntegratedTest : ConsumerBaseTest<ToDoItemQueueCreateCommand>
{
    

    public WorkFlowIntegratedTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddAndGetTodos()
    {
        var t = await CallHttp("/Todo").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult<ToDoItemEntity>();

        var tResult = await CallHttp("/Todo").GetAsync().ReceiveResult<List<ToDoItemEntity>>();

        Assert.True(t.IsSuccess);
        Assert.True(tResult.IsSuccess);
        Assert.NotEmpty(t.Value.Name);
        Assert.NotEmpty(tResult.Value);
    }


    [Fact]
    public async Task AddQueueAndGetTodos()
    {

        var t = await CallHttp("/Todo/Queue").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult();

        Assert.True(t.IsSuccess);

    }

    public override void MockMessage()
    {
        Context.Message.Returns(new ToDoItemQueueCreateCommand
        {
            Deadline = DateTime.Now.AddDays(4),
            Name = "Comprar ovos",
            CorrelationId = Guid.NewGuid()
        });
    }

    protected override IConsumer<ToDoItemQueueCreateCommand> BuildConsumer()
    {
        return Factory.Services.GetRequiredService<IConsumer<ToDoItemQueueCreateCommand>>();
    }
}