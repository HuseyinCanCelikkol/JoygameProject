using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Wrappers;
using JoygameProject.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace JoygameProject.Application.Features.Commands.Login
{
    public class ForgotPasswordCommandHandler(IUnitOfWork worker, IMailService mailService, IJwtService jwtService, IConfiguration config) : IRequestHandler<ForgotPasswordCommandRequest, ServiceResponse<string>>
    {

        public async Task<ServiceResponse<string>> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            var res = new ServiceResponse<string>();

            var user = await worker.Read<User>().GetFirstAsync(x => x.Email == request.Email);
            if (user == null)
                return res.NotFound("Kullanıcı bulunamadı");

            var token = jwtService.GenerateResetToken(user);

            var resetUrl = $"https://localhost:7007/ResetPassword?token={HttpUtility.UrlEncode(token)}";

            await mailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi Sıfırlamak için lütfen linke tıklayınız: {resetUrl}");

            return res.Success("E-Posta Gönderildi. Lütfen Gelen kutunuzu kontrol edin.");
        }
    }
}
