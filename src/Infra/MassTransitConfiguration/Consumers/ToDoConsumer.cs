using Domain.Commands;
using Domain.Contracts.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infra.MassTransitConfiguration.Consumers;

public class ToDoConsumer : IConsumer<ToDoItemQueueCreateCommand>
{
    private readonly IToDoListService _toDoListService;
    private readonly ILogger<ToDoConsumer> _logger;

    public ToDoConsumer(ILogger<ToDoConsumer> logger, IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ToDoItemQueueCreateCommand> context)
    {
        _logger.LogInformation("Mensagem recebida {@Message}", context.Message);
        var entity = await _toDoListService.Create(new ToDoItemCreateCommand
        {
            Deadline = context.Message.Deadline,
            Name = context.Message.Name
        });
        _logger.LogInformation("Tarefa cadastrada {@Message}", entity);

        await context.NotifyConsumed(context, TimeSpan.Zero, nameof(ToDoConsumer));
        
        _logger.LogInformation("Mensagem concluida {@Message}", context.Message);
    }
}