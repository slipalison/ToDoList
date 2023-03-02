using Domain.Commands;
using Domain.Contracts.Services;
using Domain.ToDoItems;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoListService _toDoListService;
    private readonly ILogger<ToDoController> _logger;

    public ToDoController(IToDoListService toDoListService, ILogger<ToDoController> logger)
    {
        _logger = logger;
        _toDoListService = toDoListService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemEntity>>> GetAll()
    {
        var list = await _toDoListService.GetAll();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default)
    {
        await _toDoListService.Create(createCommand, cancellationToken);
        return Ok();
    }
    
    [HttpPost("queue")]
    public async Task<IActionResult> CreateWithQueue([FromBody] ToDoItemCreateCommand createCommand, CancellationToken cancellationToken = default)
    {
        await _toDoListService.CreateWithQueue(createCommand, cancellationToken);
        return Ok();
    }

}