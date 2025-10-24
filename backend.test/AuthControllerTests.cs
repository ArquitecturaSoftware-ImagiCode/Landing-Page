using System.Threading.Tasks;
using backend.Controllers;
using backend.Db;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var db = GetInMemoryDb();
            var orgServiceMock = new Mock<IOrganizationService>(); // <- usar la interfaz
            db.Users.Add(new User { ClerkUserId = "user123", Email = "test@test.com" });
            await db.SaveChangesAsync();

            var controller = new AuthController(db, orgServiceMock.Object);
            var request = new RegisterRequest
            {
                ClerkUserId = "user123",
                Email = "nuevo@test.com",
                OrganizationName = "Org1",
                ClerkOrgId = "org123"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }


        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserDoesNotExist()
        {
            // Arrange
            var db = GetInMemoryDb();
            var orgServiceMock = new Mock<IOrganizationService>();
            orgServiceMock
            .Setup(s => s.AddOrganizationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new Organization());

            var controller = new AuthController(db, orgServiceMock.Object);
            var request = new RegisterRequest
            {
                ClerkUserId = "user456",
                Email = "nuevo@test.com",
                OrganizationName = "Org1",
                ClerkOrgId = "org456"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            orgServiceMock.Verify(s => s.AddOrganizationAsync(request.ClerkOrgId, request.OrganizationName, It.IsAny<int>()), Times.Once);
        }

        //caso 1
        [Fact]
        public async Task Register_ShouldReturnInternalServerError_WhenOrgServiceFails()
        {
            // Arrange
            var db = GetInMemoryDb();
            var orgServiceMock = new Mock<IOrganizationService>();

            // Simulamos que el servicio lanza un error
            orgServiceMock
                .Setup(s => s.AddOrganizationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Falla en servicio externo"));

            var controller = new AuthController(db, orgServiceMock.Object);
            var request = new RegisterRequest
            {
                ClerkUserId = "user789",
                Email = "test3@test.com",
                OrganizationName = "OrgCrash",
                ClerkOrgId = "org789"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        // caso 2
        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var db = GetInMemoryDb();
            var orgServiceMock = new Mock<IOrganizationService>();
            var controller = new AuthController(db, orgServiceMock.Object);

            // Simulamos una petición con datos faltantes
            var request = new RegisterRequest
            {
                ClerkUserId = "",
                Email = "",
                OrganizationName = "",
                ClerkOrgId = ""
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }
        //Caso 3
        [Fact]
        public async Task Register_ShouldPersistUserInDatabase_WhenSuccessful()
        {
            // Arrange
            var db = GetInMemoryDb();
            var orgServiceMock = new Mock<IOrganizationService>();
            orgServiceMock
                .Setup(s => s.AddOrganizationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new Organization());

            var controller = new AuthController(db, orgServiceMock.Object);
            var request = new RegisterRequest
            {
                ClerkUserId = "user001",
                Email = "persist@test.com",
                OrganizationName = "OrgPersist",
                ClerkOrgId = "org001"
            };

            // Act
            await controller.Register(request);

            // Assert
            var user = await db.Users.FirstOrDefaultAsync(u => u.ClerkUserId == "user001");
            Assert.NotNull(user);
            Assert.Equal("persist@test.com", user.Email);
        }
    
    }
}
