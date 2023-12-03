using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Exceptions;

namespace Shop.WebApi.Controllers;
[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult<AuthResultDto>> SignUp(SignUpCommand request)
    {
        try
        {
            return await _sender.Send(request);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<AuthResultDto>> SignIn(SignInCommand request)
    {
        try
        {
            return await _sender.Send(request);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResultDto>> RefreshToken(RefreshTokenCommand request)
    {
        try
        {
            return await _sender.Send(request);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    
}
