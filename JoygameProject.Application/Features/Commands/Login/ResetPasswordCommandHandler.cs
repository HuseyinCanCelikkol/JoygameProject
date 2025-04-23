using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Helpers;
using JoygameProject.Application.Wrappers;
using JoygameProject.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JoygameProject.Application.Features.Commands.Login
{
    public class ResetPasswordCommandHandler(IUnitOfWork worker, IConfiguration config) : IRequestHandler<ResetPasswordCommandRequest, ServiceResponse<bool>>
    {

        public async Task<ServiceResponse<bool>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var res = new ServiceResponse<bool>();

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

            var principal = handler.ValidateToken(request.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var user = await worker.Read<User>().GetByIdAsync(userId);
            if (user == null)
                return res.NotFound();

            user.HashedPassword = HashingHelper.Hash(request.NewPassword);
            worker.Write<User>().Update(user);
            await worker.SaveAsync();

            return res.Success(true);
            
        }
    }
}
