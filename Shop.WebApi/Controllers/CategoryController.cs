using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Category.Commands.CreateCategory;
using Shop.Application.Category.Commands.DeleteCategory;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Exceptions;
using Shop.Domain.Constants;

namespace Shop.WebApi.Controllers;

[ApiController]
[Route("api/category")]
//[Authorize(Policy = Policies.ManagmentCenter)]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetMainCategoriesQuery query)
    {
        try
        {
            return Ok(await _sender.Send(query));
        }
        catch (FluentValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        try
        {
            return Ok(await _sender.Send(new GetCategoryQuery { Id = id }));
        }
        catch (FluentValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        try
        {
            var result = await _sender.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        catch (FluentValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromBody] EditCategoryCommand command)
    {
        try
        {
            await _sender.Send(command);
            return NoContent();
        }
        catch (FluentValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _sender.Send(new DeleteCategoryCommand {  Id = id });
            return NoContent();
        }
        catch (FluentValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
