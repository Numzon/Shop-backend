using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Common.Exceptions;
using Shop.Application.Roles.Queries;
using Shop.Domain.Constants;

namespace Shop.WebApi.Controllers;
[ApiController]
[Route("api/role")]
[Authorize(Policy = Policies.SuperAdminOnly)]
public class RoleController : ControllerBase
{
    private readonly ISender _sender;

    public RoleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetRolesQuery query)
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

    //[HttpGet]
    //[Route("{id}")]
    //public async Task<IActionResult> Get(string id)
    //{
    //    try
    //    {
    //        //return Ok(await _sender.Send(query));
    //        return null;
    //    }
    //    catch (FluentValidationException ex)
    //    {
    //        return BadRequest(ex.Errors);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Problem(ex.Message);
    //    }
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    //{
    //    try
    //    {
    //        var result = await _sender.Send(command);
    //        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    //    }
    //    catch (FluentValidationException ex)
    //    {
    //        return BadRequest(ex.Errors);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Problem(ex.Message);
    //    }
    //}
}
