using Application.Features.Customers.Commands.CreateCustomer;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion("1.0")]
public class CustomerController : BaseApiController
{

    [HttpPost]
    public async Task<ActionResult> GetCustomer([FromBody] CreateCustomerCommand cmd)
    {
        return Ok(await Mediator.Send(cmd));
    }
  
}