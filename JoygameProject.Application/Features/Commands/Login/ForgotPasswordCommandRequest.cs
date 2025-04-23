using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Login
{
    public class ForgotPasswordCommandRequest : IRequest<ServiceResponse<string>>
    {
        public required string Email { get; set; }
    }
}
