using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Login
{
    public class LoginCommandRequest : LoginDto,IRequest<ServiceResponse<LoginResponseDto>>
    {
    }
}
