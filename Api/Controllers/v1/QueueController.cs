using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Customers.Queries.GetQueueRequest;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class QueueController : BaseApiController
    {
        [HttpGet("queue-request")]
        public async Task<IActionResult> GetQueueRequest()
        {
            var response = await Mediator.Send(new GetQueueRequestQuery());
            return Ok(new { queue = response });
        }
    }
}