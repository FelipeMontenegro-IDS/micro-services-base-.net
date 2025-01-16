using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Customers.Queries.GenerateUrlTemporal;
using Application.Interfaces.Common;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BlobStorageController : BaseApiController
    {
        [HttpGet("get-file-temporal")]
        public async Task<ActionResult<UrlTemporal>> GetFileTemporal()
        {
            var response = await Mediator.Send(new GenerateUrlTemporalQuery());
            return Ok(response);
        }
    }
}
