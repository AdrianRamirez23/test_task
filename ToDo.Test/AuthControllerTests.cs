using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using ToDo.API.Controllers;
using ToDo.Application.Interfaces;
using ToDo.Infraestructure.Dtos;

namespace ToDo.Test
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthenticationController _authController;
        private readonly Mock<ITokenService> _mockTokenService;

        public AuthControllerTests()
        {

            _mockConfiguration = new Mock<IConfiguration>();
            _mockTokenService = new Mock<ITokenService>();
            _mockConfiguration.Setup(config => config["JwtSettings:Issuer"]).Returns("TaskToDoAPI");
            _mockConfiguration.Setup(config => config["JwtSettings:Audience"]).Returns("TaskToDoUsers");
            _mockConfiguration.Setup(config => config["JwtSettings:SecretKey"]).Returns("SuperSecureKeyForTaskToDoAPI123456!");

            _mockTokenService
          .Setup(service => service.GetToken(It.IsAny<string>()))
          .Returns("MockedToken123456");
            _authController = new AuthenticationController(_mockConfiguration.Object, _mockTokenService.Object);



        }
        [Fact]
        public void Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "password"
            };

            // Act
            var result = _authController.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "wrongUser",
                Password = "wrongPassword"
            };

            // Act
            var result = _authController.Login(loginDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("Invalid credentials.", result.Value);
        }

        [Fact]
        public void Login_NullDto_ReturnsBadRequest()
        {
            // Act
            var result = _authController.Login(null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Login_EmptyUsername_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "",
                Password = "password"
            };

            // Simular modelo inválido
            _authController.ModelState.AddModelError("Username", "The Username field is required.");

            // Act
            var result = _authController.Login(loginDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            var errors = result.Value as SerializableError;
            Assert.Contains("Username", errors.Keys);
        }

        [Fact]
        public void Login_EmptyPassword_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = ""
            };

            // Simular modelo inválido
            _authController.ModelState.AddModelError("Password", "The Password field is required.");

            // Act
            var result = _authController.Login(loginDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            var errors = result.Value as SerializableError;
            Assert.Contains("Password", errors.Keys);
        }

    }
}
