using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Common.Exceptions;
using Shop.Application.SpecificationPatterns.Commands.CreateSpecificationPattern;
using Shop.Application.SpecificationPatterns.Commands.DeleteSpecificationPattern;
using Shop.Application.SpecificationPatterns.Commands.EditSpecificationPattern;
using Shop.Application.SpecificationPatterns.Queries;

namespace Shop.WebApi.Controllers;
[ApiController]
[Route("api/specification-pattern")]
public class SpecificationPatternController : ControllerBase
{
    private readonly ISender _sender;

    public SpecificationPatternController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllSpecificationPatternsQuery query)
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
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            return Ok(await _sender.Send(new GetSpecificationPatternQuery { Id = id }));
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
    [Route("select-list")]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _sender.Send(new GetSpecificationPatternsForSelectListQuery()));
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
    public async Task<IActionResult> Create([FromBody] CreateSpecificationPatternCommand command)
    {
        try
        {
            return Ok(await _sender.Send(command));
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
    public async Task<IActionResult> Edit([FromBody] EditSpecificationPatternCommand command)
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
            await _sender.Send(new DeleteSpecificationPatternCommand { Id = id });
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
