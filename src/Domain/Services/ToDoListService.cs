using Domain.Commands;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.ToDoItems;

namespace Domain.Services;

public class ToDoListService : IToDoListService
{
    private readonly IToDoListRepository _toDoListRepository;
    private readonly IToDoListPublisher<ToDoItemQueueCreateCommand> _queuePublisher;
    private readonly ICorrelationContextService _correlationContextService;

    public ToDoListService(IToDoListRepository toDoListRepository,
        IToDoListPublisher<ToDoItemQueueCreateCommand> queuePublisher,
        ICorrelationContextService correlationContextService)
    {
        _toDoListRepository = toDoListRepository;
        _queuePublisher = queuePublisher;
        _correlationContextService = correlationContextService;
    }

    public Task<List<ToDoItemEntity>> GetAll()
    {
        return _toDoListRepository.GetAll();
    }

    public Task<ToDoItemEntity> Create(ToDoItemCreateCommand createCommand,
        CancellationToken cancellationToken = default)
    {
        var entity = ToDoItemEntity.Create(createCommand.Name, createCommand.Deadline);

        return _toDoListRepository.Create(entity, cancellationToken);
    }

    public Task CreateWithQueue(ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default)
    {
        return _queuePublisher.PublishAsync(
            ToDoItemQueueCreateCommand.Create(createCommand, _correlationContextService.GetCorrelationId()));
    }
}