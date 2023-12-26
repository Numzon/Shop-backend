using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Category.Queries;
using Shop.Application.Common.Exceptions;
using Shop.Application.SpecificationTypes.Commands.CreateSpecificationType;
using Shop.Application.SpecificationTypes.Commands.DeleteSpecificationType;
using Shop.Application.SpecificationTypes.Commands.EditSpecificationType;

namespace Shop.WebApi.Controllers;

[ApiController]
[Route("api/specification-type")]
public class SpecificationTypeController : ControllerBase
{
    private readonly ISender _sender;

    public SpecificationTypeController(ISender sender)
	{
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetMainSpecificationType([FromQuery] GetMainSpecificationTypeQuery query)
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
    public async Task<IActionResult> CreateSpecificationType(CreateSpecificationTypeCommand command)
    {
        try
        {
            var result = await _sender.Send(command);
            return CreatedAtAction(nameof(EditSpecificationType), new { id = result.Id }, result);
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
    public async Task<IActionResult> EditSpecificationType(EditSpecificationTypeCommand command)
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
    public async Task<IActionResult> DeleteSpecificationType(DeleteSpecificationTypeCommand command)
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
