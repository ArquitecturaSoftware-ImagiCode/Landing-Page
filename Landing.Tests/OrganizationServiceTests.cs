using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using backend.Models;
using backend.Db;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Landing.Tests;

public class OrganizationServiceTests : IAsyncLifetime
{
    private AppDbContext _context;
    private OrganizationService _service;
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
        _context = new AppDbContext(options);
        _service = new OrganizationService(_context);

        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task AddOrganizationAsync_ShouldAddOrganizationAndSaveChanges()
    {
        //Arrange
        var clerkOrgId = "org_123";
        var name = "Test Org";
        var OwnerId = 1;

        //Act
        var result = await _service.AddOrganizationAsync(clerkOrgId, name, OwnerId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(clerkOrgId, result.ClerkOrgId);
        Assert.Equal(name, result.Name);
        Assert.Equal(OwnerId, result.OwnerId);
        Assert.False(result.IsActive);//Falso por defecto

        //Verificar que se guarda en la DB
        var orgInDb = await _context.Organizations.FirstOrDefaultAsync(o =>o.ClerkOrgId == clerkOrgId);
        Assert.NotNull(orgInDb);
        Assert.Equal(name, orgInDb.Name);
    }

    [Fact]
    public async Task ActivateOrganizationAsync_WhenOrgExists_ShouldActivateAndReturnTrue()
    {
        //Arrange
        var clerkOrgId = "org_active";
        _context.Organizations.Add(new Organization{ ClerkOrgId=clerkOrgId, Name="Active Org", OwnerId=1, IsActive=false});
        await _context.SaveChangesAsync();

        //Act
        var result = await _service.ActivateOrganizationAsync(clerkOrgId);

        //Assert
        Assert.True(result);
        var orgInDb = await _context.Organizations.FirstOrDefaultAsync(o => o.ClerkOrgId==clerkOrgId);
        Assert.True(orgInDb.IsActive);
        Assert.NotNull(orgInDb.UpdatedAt);
    }
    [Fact]
        public async Task ActivateOrganizationAsync_WhenOrgDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentClerkOrgId = "org_999";

            // Act
            var result = await _service.ActivateOrganizationAsync(nonExistentClerkOrgId);

            // Assert
            Assert.False(result);
        }
        // ---------------------------------------------------------
// 4. Desactivar Organización
// ---------------------------------------------------------

        [Fact]
        public async Task DeactivateOrganizationAsync_WhenOrgExists_ShouldDeactivateAndReturnTrue()
        {
            // Arrange
            var clerkOrgId = "org_deactivate";
            _context.Organizations.Add(new Organization { ClerkOrgId = clerkOrgId, Name = "Deact Org", OwnerId = 2, IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeactivateOrganizationAsync(clerkOrgId);

            // Assert
            Assert.True(result);
            var orgInDb = await _context.Organizations.FirstOrDefaultAsync(o => o.ClerkOrgId == clerkOrgId);
            Assert.False(orgInDb.IsActive);
            Assert.NotNull(orgInDb.UpdatedAt);
        }

        [Fact]
        public async Task DeactivateOrganizationAsync_WhenOrgDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentClerkOrgId = "org_998";

            // Act
            var result = await _service.DeactivateOrganizationAsync(nonExistentClerkOrgId);

            // Assert
            Assert.False(result);
        }

// ---------------------------------------------------------
// 5. Obtener Organización por Clerk ID
// ---------------------------------------------------------

        [Fact]
        public async Task GetOrganizationByClerkIdAsync_WhenOrgExists_ShouldReturnOrganization()
        {
            // Arrange
            var clerkOrgId = "org_get";
            _context.Organizations.Add(new Organization { ClerkOrgId = clerkOrgId, Name = "Get Org", OwnerId = 3, IsActive = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetOrganizationByClerkIdAsync(clerkOrgId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clerkOrgId, result.ClerkOrgId);
        }

        [Fact]
        public async Task GetOrganizationByClerkIdAsync_WhenOrgDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var nonExistentClerkOrgId = "org_997";

            // Act
            var result = await _service.GetOrganizationByClerkIdAsync(nonExistentClerkOrgId);

            // Assert
            Assert.Null(result);
        }

// ---------------------------------------------------------
// 6. Obtener Todas las Organizaciones
// ---------------------------------------------------------

        [Fact]
        public async Task GetOrganizations_ShouldReturnAllOrganizationsOrderedByCreation()
        {
            // Arrange
            _context.Organizations.Add(new Organization { ClerkOrgId = "org_old", Name = "Old Org", OwnerId = 4, CreatedAt = DateTime.UtcNow.AddHours(-2) });
            _context.Organizations.Add(new Organization { ClerkOrgId = "org_new", Name = "New Org", OwnerId = 4, CreatedAt = DateTime.UtcNow.AddHours(-1) });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetOrganizations();

            // Assert
            Assert.Equal(2, result.Count);
            // Verificar orden: El más reciente (New Org) debe ser el primero
            Assert.Equal("org_new", result.First().ClerkOrgId);
            Assert.Equal("org_old", result.Last().ClerkOrgId);
        }

// ---------------------------------------------------------
// 7. Obtener Organizaciones de un Usuario
// ---------------------------------------------------------

        [Fact]
        public async Task GetUserOrganizationsAsync_ShouldReturnOnlyOrganizationsForGivenOwnerId()
        {
            // Arrange
            var ownerId = 5;
            _context.Organizations.Add(new Organization { ClerkOrgId = "org_user_5_a", Name = "User 5 A", OwnerId = ownerId, CreatedAt = DateTime.UtcNow.AddMinutes(-5) });
            _context.Organizations.Add(new Organization { ClerkOrgId = "org_user_5_b", Name = "User 5 B", OwnerId = ownerId, CreatedAt = DateTime.UtcNow.AddMinutes(-1) });
            _context.Organizations.Add(new Organization { ClerkOrgId = "org_user_other", Name = "Other User", OwnerId = 6, CreatedAt = DateTime.UtcNow.AddMinutes(-10) });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetUserOrganizationsAsync(ownerId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.All(o => o.OwnerId == ownerId));
            // Verificar orden: User 5 B (más reciente) debe ser el primero
            Assert.Equal("org_user_5_b", result.First().ClerkOrgId);
        }

// ---------------------------------------------------------
// 8. Verificar Existencia de Organización
// ---------------------------------------------------------

        [Fact]
        public async Task OrganizationExistsAsync_WhenOrgExists_ShouldReturnTrue()
        {
            // Arrange
            var clerkOrgId = "org_exists";
            _context.Organizations.Add(new Organization { ClerkOrgId = clerkOrgId, Name = "Exists Org", OwnerId = 7 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.OrganizationExistsAsync(clerkOrgId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task OrganizationExistsAsync_WhenOrgDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentClerkOrgId = "org_996";

            // Act
            var result = await _service.OrganizationExistsAsync(nonExistentClerkOrgId);

            // Assert
            Assert.False(result);
        }
}
