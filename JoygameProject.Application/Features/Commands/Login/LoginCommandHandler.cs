using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Helpers;
using JoygameProject.Application.Wrappers;
using JoygameProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoygameProject.Application.Features.Commands.Login
{
    public class LoginCommandHandler(IJwtService jwtservice,IUnitOfWork worker) : IRequestHandler<LoginCommandRequest, ServiceResponse<LoginResponseDto>>
    {

        public async Task<ServiceResponse<LoginResponseDto>> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<LoginResponseDto> res = new();

            var user = await worker.Read<User>().GetFirstAsync(x =>
                x.Email == request.Email &&
                x.HashedPassword == HashingHelper.Hash(request.Password),
                 include: x => x.Include(x => x.Role).ThenInclude(x => x.RoleDetails).ThenInclude(x => x.Page));
            
            if (user == null)
                return res.NotFound();

            Dictionary<string, PermissionDto> permissions = user.Role!.RoleDetails!
                .Where(rd => rd.Page != null)
                .ToDictionary(
                    rd => rd.Page!.Code.ToLower(),
                    rd => new PermissionDto
                    {
                        CanView = rd.CanView, 
                        CanInsert = rd.CanInsert, 
                        CanUpdate = rd.CanUpdate, 
                        CanDelete = rd.CanDelete
                    }
                );

            string token = jwtservice.GenerateToken(user.Id, user.RoleId, permissions);

            return res.Success(new LoginResponseDto
            {
                Token = token
            });
        }
    }
}
