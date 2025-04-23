using JoygameProject.Application.Features.Commands.Login;
using JoygameProject.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JoygameProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
        {
            var response = await mediator.Send(request);
            if (response.Status != HttpStatusCode.OK)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ServiceResponse<string>> ForgotPassword([FromBody] ForgotPasswordCommandRequest request)
        {
            return await mediator.Send(request);
        }
    }
}
