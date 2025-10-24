using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests
{
    public class OrganizationControllerTests
    {
        private readonly Mock<IOrganizationService> _mockService;
        private readonly OrganizationController _controller;

        public OrganizationControllerTests()
        {
            _mockService = new Mock<IOrganizationService>();
            _controller = new OrganizationController(_mockService.Object);
        }

        [Fact]
        public async Task GetOrganizationByClerkId_ShouldReturnOk_WhenOrganizationExists()
        {
            // Arrange
            var org = new Organization { Id = 1, Name = "GammaLab", ClerkOrgId = "org_123" };
            _mockService.Setup(s => s.GetOrganizationByClerkIdAsync("org_123"))
                        .ReturnsAsync(org);

            // Act
            var result = await _controller.GetOrganizationByClerkId("org_123");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrg = Assert.IsType<Organization>(okResult.Value);
            Assert.Equal("GammaLab", returnedOrg.Name);
        }

        [Fact]
        public async Task GetOrganizationByClerkId_ShouldReturnNotFound_WhenOrganizationDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetOrganizationByClerkIdAsync("org_404"))
                        .ReturnsAsync((Organization)null);

            // Act
            var result = await _controller.GetOrganizationByClerkId("org_404");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task GetAllOrganizations_ShouldReturnOk_WithListOfOrganizations()
        {
            // Arrange
            var organizations = new List<Organization>
            {
                new Organization { Id = 1, Name = "GammaLab", ClerkOrgId = "org_1" },
                new Organization { Id = 2, Name = "NeuroTech", ClerkOrgId = "org_2" }
            };

            _mockService.Setup(s => s.GetOrganizations()).ReturnsAsync(organizations);

            // Act
            var result = await _controller.GetAllOrganizations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<Organization>>(okResult.Value);
            Assert.Equal(2, list.Count);
        }
    }
}
