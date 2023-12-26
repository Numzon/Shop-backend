using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Category.Commands.CreateCategory;
using Shop.Application.Category.Commands.DeleteCategory;
using Shop.Application.Category.Commands.EditCategory;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Exceptions;

namespace Shop.WebApi.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
	{
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetMainCategories([FromQuery] GetMainCategoriesQuery query)
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

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        try
        {
            var result = await _sender.Send(command);
            return CreatedAtAction(nameof(EditCategory), new { id = result.Id }, result);
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
    public async Task<IActionResult> EditCategory(EditCategoryCommand command)
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
    [Route("delete")]
    public async Task<IActionResult> DeleteCategory(DeleteCategoryCommand command)
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
}
