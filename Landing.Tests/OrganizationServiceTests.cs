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
}
