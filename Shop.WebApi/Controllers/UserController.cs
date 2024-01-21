using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Common.Exceptions;
using Shop.Application.Users.Commands.ChangeEmail;
using Shop.Application.Users.Queries;
using Shop.Domain.Constants;

namespace Shop.WebApi.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Policy = Policies.UserAccess)]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetUsersQuery query)
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
    [Route("current-user")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var id = User.Claims.FirstOrDefault(i => i.Type == CustomClaimNames.Id);

            if (id == null)
                return BadRequest();

            return Ok(await _sender.Send(new GetUserQuery { Id = id.Value }));
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
    [Route("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailCommand command)
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
}
