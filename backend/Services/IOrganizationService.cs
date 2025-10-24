using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IOrganizationService
    {
        Task<Organization> AddOrganizationAsync(string clerkOrgId, string name, int ownerId);
        Task<bool> ActivateOrganizationAsync(string clerkOrgId);
        Task<bool> DeactivateOrganizationAsync(string clerkOrgId);
        Task<Organization?> GetOrganizationByClerkIdAsync(string clerkOrgId);
        Task<List<Organization>> GetOrganizations();
        Task<List<Organization>> GetUserOrganizationsAsync(int ownerId);
        Task<bool> OrganizationExistsAsync(string clerkOrgId);
    }
}
