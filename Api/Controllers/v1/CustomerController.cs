using Application.Features.customers.Commands.CreateCustomer;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiVersion("1.0")]
public class CustomerController : BaseApiController
{

    [HttpPost]
    public async Task<ActionResult> GetCustomer([FromBody] CreateCustomerCommand cmd)
    {
        return Ok(await Mediator.Send(cmd));
    }
  
}