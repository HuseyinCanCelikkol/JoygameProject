using JoygameProject.Application.DTOs;
using JoygameProject.Domain.Entities;

namespace JoygameProject.Application.Abstractions.Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, int roleId, Dictionary<string, PermissionDto> permissions);
        string GenerateResetToken(User user);
    }
}
