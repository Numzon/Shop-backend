using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart.Commands.AddProduct;
using Shop.Application.Cart.Commands.ReduceQuantity;
using Shop.Application.Cart.Commands.RemoveProduct;
using Shop.Application.Cart.Queries;
using Shop.Application.Common.Exceptions;

namespace Shop.WebApi.Controllers;
[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ISender _sender;

    public CartController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        try
        {
            return Ok(await _sender.Send(new GetCartQuery { CartId = id }));
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
    [Route("product-details/{id}")]
    public async Task<IActionResult> GetCartProdutsDetails([FromRoute] Guid id)
    {
        try
        {
            return Ok(await _sender.Send(new GetCartProductsDetailsQuery { Id = id }));
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
    [Route("add-product")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductToCartCommand command)
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
    [Route("set-quantity")]
    public async Task<IActionResult> SetQuantity([FromBody] SetQuantityCommand command)
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
    [Route("remove-product")]
    public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductCommand command)
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
