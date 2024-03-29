﻿using Domain.Commands;
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
    public async Task<ActionResult<ToDoItemEntity>> Create([FromBody] ToDoItemCreateCommand createCommand,
        CancellationToken cancellationToken = default)
    {
        if (!createCommand.Validate().IsSuccess)
            return BadRequest(createCommand.Validate().Error);

        var result = await _toDoListService.Create(createCommand, cancellationToken);
        return Ok(result);
    }

    [HttpPost("queue")]
    public async Task<IActionResult> CreateWithQueue([FromBody] ToDoItemCreateCommand createCommand,
        CancellationToken cancellationToken = default)
    {
        if (!createCommand.Validate().IsSuccess)
            return BadRequest(createCommand.Validate().Error);

        await _toDoListService.CreateWithQueue(createCommand, cancellationToken);
        _logger.LogInformation("Registor enviado para cadastro");
        return Ok();
    }

    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<ToDoItemEntity>> Update([FromRoute] Guid id, [FromBody] UpdateCommad updateCommad,
        CancellationToken cancellationToken = default)
    {
        if (!updateCommad.Validate().IsSuccess)
            return BadRequest(updateCommad.Validate().Error);

        var result = await _toDoListService.Update(id, updateCommad, cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        _logger.LogInformation("Registor atulalizado");
        return BadRequest(result.Error);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _toDoListService.Delete(id, cancellationToken);
        if (result.IsSuccess)
            return Ok();
        _logger.LogInformation("Registor apagado");
        return BadRequest(result.Error);
    }
}