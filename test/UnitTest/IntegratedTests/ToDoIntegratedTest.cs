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
        Assert.NotEmpty(t.Value.Name!);
        Assert.NotEmpty(tResult.Value);

        Assert.Contains(t.Value.Id, tResult.Value.Select(x => x.Id));
    }


    [Fact]
    public async Task AddTaskWithError()
    {
        var t = await CallHttp("/Todo").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(-2), Name = "Co" }).ReceiveResult<ToDoItemEntity>();

        Assert.False(t.IsSuccess);
        Assert.True(t.Error.Code == "404");
        Assert.Contains("DeadLine", t.Error.Errors.Select(x => x.Key));
        Assert.Contains("Name", t.Error.Errors.Select(x => x.Key));
    }


    [Fact]
    public async Task AddQueueAndGetTodos()
    {
        var t = await CallHttp("/Todo/Queue").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult();

        Assert.True(t.IsSuccess);
    }


    [Fact]
    public async Task Update()
    {
        var created = await CallHttp("/Todo").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult<ToDoItemEntity>();
        Assert.True(created.IsSuccess);
        Assert.True(created.Value.Name == "Comprar ovos");
        Assert.True(created.Value.Status == Status.ToDo);


        var updated = await CallHttp($"/Todo/{created.Value.Id}").PatchJsonAsync(new UpdateCommad
            { Name = "Comprar ovos2", Status = Status.InProgress }).ReceiveResult<ToDoItemEntity>();

        Assert.True(updated.IsSuccess);
        Assert.True(updated.Value.Name == "Comprar ovos2");
        Assert.True(updated.Value.Status == Status.InProgress);
    }

    
    [Fact]
    public async Task UpdateWithError()
    {
        var created = await CallHttp("/Todo").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult<ToDoItemEntity>();
        Assert.True(created.IsSuccess);
        Assert.True(created.Value.Name == "Comprar ovos");
        Assert.True(created.Value.Status == Status.ToDo);


        var updated = await CallHttp($"/Todo/{created.Value.Id}").PatchJsonAsync(new UpdateCommad
            { Name = "Co", Status = Status.InProgress }).ReceiveResult<ToDoItemEntity>();

        Assert.False(updated.IsSuccess);
        Assert.Contains(updated.Error.Errors, x => x.Key == "Name");

    }

    

    [Fact]
    public async Task Delete()
    {
        var created = await CallHttp("/Todo").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(2), Name = "Comprar ovos" }).ReceiveResult<ToDoItemEntity>();
        Assert.True(created.IsSuccess);

        var updated = await CallHttp($"/Todo/{created.Value.Id}").DeleteAsync().ReceiveResult();
        Assert.True(updated.IsSuccess);
        
        var lResult = await CallHttp("/Todo").GetAsync().ReceiveResult<List<ToDoItemEntity>>();
        
        Assert.True(lResult.IsSuccess);
        Assert.DoesNotContain(lResult.Value, x =>x.Id == created.Value.Id);
        
        
    }


    [Fact]
    public async Task AddQueueTaskWithError()
    {
        var t = await CallHttp("/Todo/Queue").PostJsonAsync(new ToDoItemCreateCommand
            { Deadline = DateTime.Now.AddDays(-2), Name = "Co" }).ReceiveResult<ToDoItemEntity>();

        Assert.False(t.IsSuccess);
        Assert.True(t.Error.Code == "404");
        Assert.Contains("DeadLine", t.Error.Errors.Select(x => x.Key));
        Assert.Contains("Name", t.Error.Errors.Select(x => x.Key));
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