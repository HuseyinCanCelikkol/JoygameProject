using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Features.Commands.Login;
using JoygameProject.Application.Helpers;
using JoygameProject.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace JoygameProject.Tests.Tests.Login
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IJwtService> _jwtServiceMock = new();
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _handler = new LoginCommandHandler(_jwtServiceMock.Object, _uowMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var fakeUser = new User
            {
                Id = 1,
                Email = "test@huseyin.com",
                HashedPassword = HashingHelper.Hash("123456"),
                Role = new Role
                {
                    Id = 2,
                    Name = "TestAdmin",
                    RoleDetails = []
                }
            };

            _uowMock.Setup(x => x.Read<User>().GetFirstAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                false,
                false
            )).ReturnsAsync(fakeUser);

            _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Dictionary<string, PermissionDto>>()))
                .Returns("fake-token");

            var request = new LoginCommandRequest
            {
                Email = "test@huseyin.com",
                Password = "123456"
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Status == HttpStatusCode.OK);
            Assert.Equal("fake-token", result.Result?.Token);
        }
    }

}
