namespace JoygameProject.Application.Abstractions.Services
{
    public interface IMailService
    {
        public Task SendEmailAsync(string to, string subject, string body);
    }
}
