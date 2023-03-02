using Domain.Contracts.Services;
using Domain.ToDoItems;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ChartOfAccountsController : ControllerBase
{
    private readonly IToDoListService _toDoListService;
    private readonly ILogger<ChartOfAccountsController> _logger;

    public ChartOfAccountsController(IToDoListService toDoListService, ILogger<ChartOfAccountsController> logger)
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

}