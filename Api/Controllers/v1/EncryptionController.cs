using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Customers.Queries.DecryptText;
using Application.Features.Customers.Queries.EncryptText;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion("1.0")]
public class EncryptionController : BaseApiController
{
    [HttpGet("encrypt")]
    public async Task<IActionResult> Encrypt([FromQuery] string textToEncrypt)
    {
        string response = await Mediator.Send(new EncryptTextQuery(textToEncrypt));
        return Ok(new { textToEncrypt = response });
    }

    [HttpGet("decrypt")]
    public async Task<IActionResult> Decrypt([FromQuery] string cipherTextDecrypt)
    {
        string response = await Mediator.Send(new DecryptTextQuery(cipherTextDecrypt));
        return Ok(new { textToDecrypt = response });
    }
}