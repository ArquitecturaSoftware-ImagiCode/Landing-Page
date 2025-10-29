using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationDashboardController : ControllerBase
    {
        private readonly OrganizationDashboardService _dashboardService;

        public OrganizationDashboardController(OrganizationDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("dashboard-data")]
        public async Task<IActionResult> GetDashboardData([FromQuery] string clerkOrgId)
        {
            try
            {
                var data = await _dashboardService.GetDashboardDataAsync(clerkOrgId);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("notifications/{notificationId}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId, [FromQuery] string clerkOrgId)
        {
            try
            {
                await _dashboardService.MarkNotificationAsReadAsync(notificationId, clerkOrgId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}

